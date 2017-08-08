using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Contact
{
    public class DeleteContactMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("contacts/v1/contact/vid/61571") && request.Method == HttpMethod.Delete;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            const string jsonResponse = "{\"vid\": 61571, \"deleted\": true,\"reason\": \"OK\"}";

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent(jsonResponse),
                StatusCode = HttpStatusCode.NoContent,
                RequestMessage = request
            };

            return Task.FromResult(response);
        }
    }
}