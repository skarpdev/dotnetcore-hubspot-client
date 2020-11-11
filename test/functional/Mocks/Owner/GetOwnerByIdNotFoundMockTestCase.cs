using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Owner
{
    public class GetOwnerByIdNotFoundMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("/owners/v2/owners/158") && request.Method == HttpMethod.Get;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse = "{'status':'error','message':'resource not found','correlationId':'addfe990 - 865c - 4dca - ae27 - 7f718bd0881e','requestId':'8516ca3e2e533437cf760c0fc3546dfe'}";
            response.Content = new JsonContent(jsonResponse);
            response.StatusCode = HttpStatusCode.NotFound;
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}
