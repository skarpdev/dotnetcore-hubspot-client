using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Owner
{
    public class GetOwnerMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("/owners/v2/owners/64") && request.Method == HttpMethod.Get;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse = "{ \"portalId\": 62515, \"ownerId\": 64, \"type\": \"PERSON\", \"firstName\": \"An owner first name\", \"lastName\": \"An owner last name\", \"email\": \"owner@company.com\", \"createdAt\": 1405605858624, \"updatedAt\": 1416928146449, \"remoteList\": [ { \"portalId\": 62515, \"ownerId\": 64, \"remoteId\": \"137304\", \"remoteType\": \"HUBSPOT\", \"active\": true } ] }";
            response.Content = new JsonContent(jsonResponse);
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}
