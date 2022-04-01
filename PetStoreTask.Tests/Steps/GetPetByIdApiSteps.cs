using FluentAssertions;
using Newtonsoft.Json;
using PetStoreTask.Business;
using PetStoreTask.Business.Entities.ApiResponses;
using PetStoreTask.Business.Utils;
using RestSharp;
using TechTalk.SpecFlow;

namespace PetStoreTask.Tests.Steps
{
    [Binding]
    class GetPetByIdApiSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public GetPetByIdApiSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"Test pet is posted to the database")]
        public void VerifyPetIsPostedToDatabaseAndGetPetId()
        {
            _scenarioContext["restClient"] = new RestClient(Urls.BaseUrl);
            _scenarioContext["expectedPet"] = new PetFactory().GetTestPet();

            var request = CreateApiRequest(Method.Post, _scenarioContext.Get<Pet>("expectedPet"));

            var response = _scenarioContext.Get<RestClient>("restClient")
                .ExecutePostAsync(request);

            var responseObject = JsonConvert.DeserializeObject<Pet>(response.Result.Content);

            _scenarioContext.Get<Pet>("expectedPet").Id = responseObject.Id;

            responseObject.Should().BeEquivalentTo(_scenarioContext.Get<Pet>("expectedPet"),
                options => options
                .Excluding(field => field.Id),
                "POST action returned incorrect result.");
        }

        [When(@"GET request is sent")]
        public void SendCorrectGetRequestToGetPetByIdEndpoint()
        {
            var request = CreateApiRequest(Method.Get,
                _scenarioContext.Get<Pet>("expectedPet"),
                _scenarioContext.Get<Pet>("expectedPet").Id.ToString());

            var response = _scenarioContext.Get<RestClient>("restClient")
                .ExecuteGetAsync(request);

            _scenarioContext["petFromApi"] = JsonConvert.DeserializeObject<Pet>(response.Result.Content);
        }

        [Then(@"Correct result should be returned")]
        public void ValidateGetPetByIdEndpointReturnsCorrectResult() =>
            _scenarioContext.Get<Pet>("petFromApi")
                .Should().BeEquivalentTo(_scenarioContext.Get<Pet>("expectedPet"),
                    "GET action returned incorrect result");

        [Given(@"GET request is sent with incorrect '(.*)'")]
        public void SendIncorrectRequestToGetPetByIdEndpointAndGetErrorMessage(string id)
        {
            _scenarioContext["restClient"] = new RestClient(Urls.BaseUrl);

            var request = CreateApiRequest(Method.Get, resource: id);

            var response = _scenarioContext.Get<RestClient>("restClient")
                .ExecuteGetAsync(request);

            _scenarioContext["apiErrorMessage"] =
                JsonConvert.DeserializeObject<ErrorResponse>(response.Result.Content).Message;
        }

        [Then(@"Correct '(.*)' should be returned")]
        public void VerifyCorrectErrorMessageIsReturned(string expectedErrorMessage) =>
            _scenarioContext.Get<string>("apiErrorMessage").Should().BeEquivalentTo(expectedErrorMessage,
                "Incorrect GET action returned unexpected error message.");

        private static RestRequest CreateApiRequest(Method httpMethod, Pet? requestBody = null, string resource = "")
        {
            var request = new RestRequest()
            {
                Method = httpMethod,
                Resource = resource
            };

            if (requestBody != null)
            {
                request.AddBody(requestBody);
            }

            request.AddHeader("Content-Type", "application/json");

            return request;
        }
    }
}
