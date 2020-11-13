using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.LineItem
{
    public class CreateBatchLineItemMockTestCase: IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.EndsWith("/crm-objects/v1/objects/line_items/batch-create") && request.Method == HttpMethod.Post;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse = "[{'objectType':'LINE_ITEM','portalId':62515,'objectId':9845651,'properties':{'amount':{'value':'687.50','timestamp':0,'source':'CALCULATED','sourceId':'LineItemAmountCalculator'},'quantity':{'value':'25','timestamp':1525369755209,'source':'API','sourceId':null},'hs_lastmodifieddate':{'value':'0','timestamp':0,'source':'CALCULATED','sourceId':null},'price':{'value':'27.50','timestamp':1525299376761,'source':'API','sourceId':null},'name':{'value':'A custom name for the product for this line item.','timestamp':1525369755209,'source':'API','sourceId':null},'createdate':{'value':'0','timestamp':0,'source':'API','sourceId':null},'description':{'value':'A description of this product.','timestamp':1525299376761,'source':'API','sourceId':null},'hs_product_id':{'value':'1645342','timestamp':1525369755209,'source':'API','sourceId':null},'recurringbillingfrequency':{'value':'quarterly','timestamp':1525299376761,'source':'API','sourceId':null}},'version':0,'isDeleted':false},{'objectType':'LINE_ITEM','portalId':62515,'objectId':9867373,'properties':{'amount':{'value':'-275.00','timestamp':0,'source':'CALCULATED','sourceId':'LineItemAmountCalculator'},'quantity':{'value':'25','timestamp':1525369755209,'source':'API','sourceId':null},'hs_lastmodifieddate':{'value':'0','timestamp':0,'source':'CALCULATED','sourceId':null},'price':{'value':'9.00','timestamp':1525369755209,'source':'API','sourceId':null},'name':{'value':'Widgets, special discount price','timestamp':1525369755209,'source':'API','sourceId':null},'createdate':{'value':'0','timestamp':0,'source':'API','sourceId':null},'discount':{'value':'20','timestamp':1525292253568,'source':'API','sourceId':null},'description':{'value':'A description of yet another product.','timestamp':1525289943771,'source':'API','sourceId':null},'hs_product_id':{'value':'1645187','timestamp':1525369755209,'source':'API','sourceId':null},'recurringbillingfrequency':{'value':'annually','timestamp':1525289943771,'source':'API','sourceId':null}},'version':0,'isDeleted':false}]";
            response.Content = new JsonContent(jsonResponse);
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}
