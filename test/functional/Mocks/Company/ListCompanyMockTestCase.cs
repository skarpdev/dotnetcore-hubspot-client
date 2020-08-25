using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Company
{
    public class ListCompanyMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("/companies/v2/companies/paged");
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse =
            "{\"companies\":[" +
            "{'portalId': 62515,'companyId': 10444745,'isDeleted': false,'properties': {'description': {'value': 'A far better description than before',      'timestamp': 1403218621658,'source': 'API','sourceId': null,'versions': [{'name': 'description','value': 'A far better description than before','timestamp': 1403218621658,'source': 'API','sourceVid': []}]},'name': {'value': 'A company name','timestamp': 1403217668394,'source': 'API','sourceId': null,'versions': [{'name': 'name','value': 'A company name','timestamp': 1403217668394,'source': 'API','sourceVid': []}]},'createdate': {'value': '1403217668394','timestamp': 1403217668394,'source': 'API','sourceId': null,'versions': [{'name': 'createdate','value': '1403217668394','timestamp': 1403217668394,'source': 'API','sourceVid': []}]}}}, " +
            "{'portalId': 62515,'companyId': 10444746,'isDeleted': false,'properties': {'description': {'value': 'B far better description than before',      'timestamp': 1403218621659,'source': 'API','sourceId': null,'versions': [{'name': 'description','value': 'B far better description than before','timestamp': 1403218621658,'source': 'API','sourceVid': []}]},'name': {'value': 'B company name','timestamp': 1403217668394,'source': 'API','sourceId': null,'versions': [{'name': 'name','value': 'B company name','timestamp': 1403217668394,'source': 'API','sourceVid': []}]},'createdate': {'value': '1403217668394','timestamp': 1403217668394,'source': 'API','sourceId': null,'versions': [{'name': 'createdate','value': '1403217668394','timestamp': 1403217668394,'source': 'API','sourceVid': []}]}}}" +
            "],\"has-more\":true,\"offset\":10444746}";


            response.Content = new JsonContent(jsonResponse);
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}