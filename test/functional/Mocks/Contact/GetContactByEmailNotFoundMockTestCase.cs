using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Contact
{
    public class GetContactByEmailNotFoundMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("/contacts/v1/contact/email/not@here.com");
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse =
                "{\"status\":\"error\",\"message\":\"contact does not exist\",\"correlationId\":\"6acf1262-0456-48d8-b0f8-070a698e7d05\",\"requestId\":\"ded0ada067ad2b3ff547f46afd03b9a7\"}";
            response.Content = new JsonContent(jsonResponse);
            response.StatusCode = HttpStatusCode.NotFound;
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}