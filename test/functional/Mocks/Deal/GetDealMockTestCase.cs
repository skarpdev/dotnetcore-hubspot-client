using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Deal
{
    public class GetDealMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("/deals/v1/deal/151088") && request.Method == HttpMethod.Get;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse = "{'portalId': 62515,'dealId': 151088,'isDeleted': false,'associations': {'associatedVids': [27136],'associatedCompanyIds': [8954037],'associatedDealIds': []},'properties': {'amount': {'value': '560','timestamp': 1410381338943,'source': 'API','sourceId': null,'versions': [{'name': 'amount','value': '560','timestamp': 1410381338943,'source': 'API', 'sourceVid': []}]},'dealstage': {'value': 'appointmentscheduled','timestamp': 1410381338943,'source': 'API','sourceId': null,'versions': [{'name': 'dealstage','value': 'appointmentscheduled','timestamp': 1410381338943,'source': 'API','sourceVid': []}]},'pipeline': {'value': 'default','timestamp': 1410381338943,'source': 'API', 'sourceId': null,'versions': [{'name': 'pipeline','value': 'default','timestamp': 1410381338943,'source': 'API','sourceVid': []}]},'closedate': {'value': '1409443200000','timestamp': 1410381338943,'source': 'API','sourceId': null,'versions': [{'name': 'closedate','value': '1409443200000','timestamp': 1410381338943,'source': 'API', 'sourceVid': []}]},'createdate': {'value': '1410381339020','timestamp': 1410381339020,'source': null,'sourceId': null,'versions': [{'name': 'createdate','value': '1410381339020','timestamp': 1410381339020,'sourceVid': []}]},'hubspot_owner_id': {'value': '24','timestamp': 1410381338943,'source': 'API','sourceId': null,'versions': [{'name': 'hubspot_owner_id','value': '24','timestamp': 1410381338943,'source': 'API', 'sourceVid': []}]},'hs_createdate': {'value': '1410381339020','timestamp': 1410381339020,'source': null,'sourceId': null,'versions': [{'name': 'hs_createdate','value': '1410381339020','timestamp': 1410381339020,'sourceVid': []}]},'dealtype': {'value': 'newbusiness','timestamp': 1410381338943,'source': 'API','sourceId': null,'versions': [{'name': 'dealtype','value': 'newbusiness','timestamp': 1410381338943,'source': 'API', 'sourceVid': []}]},'dealname': {'value': 'This is a Deal','timestamp': 1410381338943,'source': 'API','sourceId': null,'versions': [{'name': 'dealname','value': 'This is a Deal','timestamp': 1410381338943,'source': 'API','sourceVid': []}]}}}";

            response.Content = new JsonContent(jsonResponse);
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}