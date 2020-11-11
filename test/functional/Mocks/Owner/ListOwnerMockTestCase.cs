using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Owner
{
    public class ListOwnerMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("/owners/v2/owners") && request.Method == HttpMethod.Get;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse =
            "[" +
            "{ \"portalId\": 62515, \"ownerId\": 64, \"type\": \"PERSON\", \"firstName\": \"Holly\", \"lastName\": \"Flax\", \"email\": \"holly@dundermifflin.com\", \"createdAt\": 1405605858624, \"updatedAt\": 1416928146449, \"remoteList\": [ { \"portalId\": 62515, \"ownerId\": 64, \"remoteId\": \"137304\", \"remoteType\": \"HUBSPOT\", \"active\": true } ] }, " +
            "{ \"portalId\": 62515, \"ownerId\": 65, \"type\": \"PERSON\", \"firstName\": \"Michael\", \"lastName\": \"Scott\", \"email\": \"michael@dundermifflin.com\", \"createdAt\": 1405605858732, \"updatedAt\": 1416928145437, \"remoteList\": [ { \"portalId\": 62515, \"ownerId\": 65, \"remoteId\": \"153112\", \"remoteType\": \"HUBSPOT\", \"active\": true } ] }, " +
            "{ \"portalId\": 62515, \"ownerId\": 66, \"type\": \"PERSON\", \"firstName\": \"Dwight\", \"lastName\": \"Schrute\", \"email\": \"dwight@dundermifflin.com\", \"createdAt\": 1405605858898, \"updatedAt\": 1416928146430, \"remoteList\": [ { \"portalId\" : 62515, \"ownerId\": 66, \"remoteId\": \"166656\", \"remoteType\": \"HUBSPOT\", \"active\": true } ] } " +
            "]";

            response.Content = new JsonContent(jsonResponse);
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}
