using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Deal
{
    public class DeleteDealMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("deals/v1/deal/151088") && request.Method == HttpMethod.Delete;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            const string jsonResponse = "";

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