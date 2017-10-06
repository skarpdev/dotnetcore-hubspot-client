using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RapidCore.Network;
using RapidCore.Threading;
using Skarp.HubSpotClient.Core;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Company
{
    public class SearchByDomainMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            var requestData = request.Content.ReadAsStringAsync().AwaitSync();
            return request.RequestUri.AbsolutePath.EndsWith("/companies/v2/domains/miaki.io/companies");
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse =
                "{\"results\":[{\"portalId\":62515,\"companyId\":184896670,\"isDeleted\":false,\"properties\":{\"hs_lastmodifieddate\":{\"value\":\"1502872954691\",\"timestamp\":1502872954691,\"source\":\"CALCULATED\",\"sourceId\":null,\"versions\":[{\"name\":\"hs_lastmodifieddate\",\"value\":\"1502872954691\",\"timestamp\":1502872954691,\"source\":\"CALCULATED\",\"sourceVid\":[]}]},\"domain\":{\"value\":\"hubspot.com\",\"timestamp\":1457708103847,\"source\":\"COMPANIES\",\"sourceId\":null,\"versions\":[{\"name\":\"domain\",\"value\":\"hubspot.com\",\"timestamp\":1457708103847,\"source\":\"COMPANIES\",\"sourceVid\":[]}]},\"name\":{\"value\":\"Hubspot, Inc.\",\"timestamp\":1457708103906,\"source\":\"BIDEN\",\"sourceId\":\"name\",\"versions\":[{\"name\":\"name\",\"value\":\"Hubspot, Inc.\",\"timestamp\":1457708103906,\"sourceId\":\"name\",\"source\":\"BIDEN\",\"sourceVid\":[]}]},\"createdate\":{\"value\":\"1457708103847\",\"timestamp\":1457708103847,\"source\":\"API\",\"sourceId\":null,\"versions\":[{\"name\":\"createdate\",\"value\":\"1457708103847\",\"timestamp\":1457708103847,\"source\":\"API\",\"sourceVid\":[]}]}},\"additionalDomains\":[],\"stateChanges\":[],\"mergeAudits\":[]},{\"portalId\":62515,\"companyId\":418736767,\"isDeleted\":false,\"properties\":{\"hs_lastmodifieddate\":{\"value\":\"1498644245669\",\"timestamp\":1498644245669,\"source\":\"CALCULATED\",\"sourceId\":null,\"versions\":[{\"name\":\"hs_lastmodifieddate\",\"value\":\"1498644245669\",\"timestamp\":1498644245669,\"source\":\"CALCULATED\",\"sourceVid\":[]}]},\"domain\":{\"value\":\"hubspot.com\",\"timestamp\":1490280388204,\"source\":\"API\",\"sourceId\":null,\"versions\":[{\"name\":\"domain\",\"value\":\"hubspot.com\",\"timestamp\":1490280388204,\"source\":\"API\",\"sourceVid\":[]}]},\"name\":{\"value\":\"qweqwe2323\",\"timestamp\":1490280388204,\"source\":\"API\",\"sourceId\":null,\"versions\":[{\"name\":\"name\",\"value\":\"qweqwe2323\",\"timestamp\":1490280388204,\"source\":\"API\",\"sourceVid\":[]}]},\"createdate\":{\"value\":\"1490280388204\",\"timestamp\":1490280388204,\"source\":\"API\",\"sourceId\":\"API\",\"versions\":[{\"name\":\"createdate\",\"value\":\"1490280388204\",\"timestamp\":1490280388204,\"sourceId\":\"API\",\"source\":\"API\",\"sourceVid\":[]}]}},\"additionalDomains\":[],\"stateChanges\":[],\"mergeAudits\":[]}],\"hasMore\":true,\"offset\":{\"companyId\":418736767,\"isPrimary\":true}}";
            response.Content = new JsonContent(jsonResponse);
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}