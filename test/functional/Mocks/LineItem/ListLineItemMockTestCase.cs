using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.LineItem
{
    public class ListLineItemMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.EndsWith("/crm-objects/v1/objects/line_items/paged")
                && request.RequestUri.Query.Contains("properties=my_custom_property")
                && request.RequestUri.Query.Contains("offset=9867220")
                && request.Method == HttpMethod.Get;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse = "{'objects':[{'objectType':'LINE_ITEM','portalId':62515,'objectId':9843068,'properties':{'hs_product_id':{'value':'1642736','timestamp':0,'source':null,'sourceId':null}},'version':2,'isDeleted':false},{'objectType':'LINE_ITEM','portalId':62515,'objectId':9867220,'properties':{'hs_product_id':{'value':'1645187','timestamp':0,'source':null,'sourceId':null}},'version':1,'isDeleted':false}],'hasMore':true,'offset':9867220}";
            response.Content = new JsonContent(jsonResponse);
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}
