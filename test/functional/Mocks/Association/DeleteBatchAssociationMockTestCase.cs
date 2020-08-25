using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Association
{
    public class DeleteBatchAssociationMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("/crm-associations/v1/associations/create-batch") && request.Method == HttpMethod.Put;
        }
        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.NoContent);

            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}
