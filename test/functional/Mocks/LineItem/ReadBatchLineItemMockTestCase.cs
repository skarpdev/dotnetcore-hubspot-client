using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.LineItem
{
    internal class ReadBatchLineItemMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.EndsWith("/crm-objects/v1/objects/line_items/batch-read") && request.Method == HttpMethod.Post;
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse = "{'9845651':{'objectType':'LINE_ITEM','portalId':62515,'objectId':9845651,'properties':{'hs_product_id':{'versions':[{'name':'hs_product_id','value':'1645342','sourceVid':[]}],'value':'1645342','timestamp':0,'source':null,'sourceId':null}},'version':1,'isDeleted':false},'9867373':{'objectType':'LINE_ITEM','portalId':62515,'objectId':9867373,'properties':{'hs_product_id':{'versions':[{'name':'hs_product_id','value':'1645187','sourceVid':[]}],'value':'1645187','timestamp':0,'source':null,'sourceId':null}},'version':1,'isDeleted':false}}";
            response.Content = new JsonContent(jsonResponse);
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}
