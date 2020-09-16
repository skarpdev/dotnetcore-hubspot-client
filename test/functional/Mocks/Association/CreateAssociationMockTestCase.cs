using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Association
{
    public class CreateAssociationMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("/crm-associations/v1/associations") && request.Method == HttpMethod.Put;
        }
        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.NoContent);

            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}
