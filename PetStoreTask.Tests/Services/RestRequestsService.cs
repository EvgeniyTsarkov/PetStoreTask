using PetStoreTask.Business;
using PetStoreTask.Business.Utils;
using RestSharp;
using System.Threading.Tasks;

namespace PetStoreTask.Tests.Services
{
    public class RestRequestsService
    {
        private static readonly RestClient _restClient = new RestClient(Urls.BaseUrl);

        public static async Task<RestResponse> SendRequestAsync(RestRequest request) =>
            await _restClient.ExecuteAsync(request);

        public static RestRequest CreateApiRequest(Method httpMethod, Pet? requestBody = null, string resource = "")
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
