using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.LineItem
{
    public class DeleteBatchLineItemMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.EndsWith("/crm-objects/v1/objects/line_items/batch-delete") && request.Method == HttpMethod.Post;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.NoContent)
            {
                RequestMessage = request
            };

            return Task.FromResult(response);
        }
    }
}
