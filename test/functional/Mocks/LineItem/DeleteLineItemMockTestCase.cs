using RapidCore.Network;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.LineItem
{
    public class DeleteLineItemMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.EndsWith("/crm-objects/v1/objects/line_items/9867220") && request.Method == HttpMethod.Delete;
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
