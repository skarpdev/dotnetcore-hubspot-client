using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.LineItem
{
    public class GetLineItemNotFoundMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("/crm-objects/v1/objects/line_items/158") && request.Method == HttpMethod.Get;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse =
                "{'status':'error','message':'Line item does not exist','correlationId':'44f0d1e5-d6d8-4bc5-9b1b-51e5064e206f','requestId':'e3efb55b4250d72680f290f22bdf423e'}";
            response.Content = new JsonContent(jsonResponse);
            response.StatusCode = HttpStatusCode.NotFound;
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}