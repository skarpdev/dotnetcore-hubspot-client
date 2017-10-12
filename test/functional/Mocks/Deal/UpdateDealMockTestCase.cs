using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Deal
{
    public class UpdateDealMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("deals/v1/deal/151088") && request.Method == HttpMethod.Put;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse = "{'portalId': 62515,'dealId': 151088,'isDeleted': false,'associations': null,'properties': {'amount': {'value': '560','timestamp': 1459873345256,'source': 'API','sourceId': null,'versions': [{'name': 'amount','value': '560','timestamp': 1459873345256,'source': 'API','sourceVid': []}]},'hs_lastmodifieddate': {'value': '1459873345260','timestamp': 1459873345260,'source': 'CALCULATED','sourceId': null,'versions': [{'name': 'hs_lastmodifieddate','value': '1459873345260','timestamp': 1459873345260,'source': 'CALCULATED','sourceVid': []}]}},'imports': []}";
            response.Content = new JsonContent(jsonResponse);
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}