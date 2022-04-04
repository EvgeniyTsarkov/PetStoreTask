using FluentAssertions;
using Newtonsoft.Json;
using PetStoreTask.Business;
using PetStoreTask.Business.Entities.ApiResponses;
using PetStoreTask.Tests.Services;
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
            _scenarioContext["expectedPet"] = new PetFactory().GetTestPet();

            var request = RestRequestsService.
                CreateApiRequest(Method.Post, _scenarioContext.Get<Pet>("expectedPet"));

            var response = RestRequestsService.SendRequestAsync(request);

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
            var request = RestRequestsService.CreateApiRequest(Method.Get,
                _scenarioContext.Get<Pet>("expectedPet"),
                _scenarioContext.Get<Pet>("expectedPet").Id.ToString());

            var response = RestRequestsService.SendRequestAsync(request);

            _scenarioContext["petFromApi"] = JsonConvert.DeserializeObject<Pet>(response.Result.Content);
        }

        [Then(@"Pet from API shall be equal to expected test pet")]
        public void ValidateGetPetByIdEndpointReturnsCorrectResult() =>
            _scenarioContext.Get<Pet>("petFromApi")
                .Should().BeEquivalentTo(_scenarioContext.Get<Pet>("expectedPet"),
                    "GET action returned incorrect result");

        [AfterScenario("deleteTestPet")]
        public void DeleteTestPet()
        {
            var request = RestRequestsService.CreateApiRequest(Method.Delete,
                resource: _scenarioContext.Get<Pet>("expectedPet").Id.ToString());

            var result = RestRequestsService.SendRequestAsync(request);

            result.Result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Given(@"GET request is sent with incorrect '(.*)'")]
        public void SendIncorrectRequestToGetPetByIdEndpointAndGetErrorMessage(string id)
        {
            var request = RestRequestsService.CreateApiRequest(Method.Get, resource: id);

            var response = RestRequestsService.SendRequestAsync(request);

            _scenarioContext["apiErrorMessage"] =
                JsonConvert.DeserializeObject<ErrorResponse>(response.Result.Content).Message;
        }

        [Then(@"Correct '(.*)' should be returned")]
        public void VerifyCorrectErrorMessageIsReturned(string expectedErrorMessage) =>
            _scenarioContext.Get<string>("apiErrorMessage").Should().BeEquivalentTo(expectedErrorMessage,
                "Incorrect GET action returned unexpected error message.");
    }
}
