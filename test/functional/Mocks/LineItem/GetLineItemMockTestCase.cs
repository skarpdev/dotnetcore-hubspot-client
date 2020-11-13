using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.LineItem
{
    public class GetLineItemMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.EndsWith("/crm-objects/v1/objects/line_items/9867220") 
                && request.RequestUri.Query.Contains("properties=my_custom_property")
                && request.RequestUri.Query.Contains("includeDeletes=true")
                && request.Method == HttpMethod.Get;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse = "{'objectType':'LINE_ITEM','portalId':62515,'objectId':9867220,'properties':{'amount':{'value':'475.00','timestamp':0,'source':'CALCULATED','sourceId':'LineItemAmountCalculator'},'quantity':{'value':'50','timestamp':1525368534412,'source':'API','sourceId':null},'hs_lastmodifieddate':{'value':'0','timestamp':0,'source':'CALCULATED','sourceId':null},'price':{'value':'9.50','timestamp':1525368534412,'source':'API','sourceId':null},'name':{'value':'This is a LineItem','timestamp':1525368534412,'source':'API','sourceId':null},'createdate':{'value':'0','timestamp':0,'source':'API','sourceId':null},'description':{'value':'This product has an updated description and price.','timestamp':1525287810508,'source':'API','sourceId':null},'hs_product_id':{'value':'1642736','timestamp':1525368534412,'source':'API','sourceId':null}},'version':0,'isDeleted':false}";
            response.Content = new JsonContent(jsonResponse);
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}
