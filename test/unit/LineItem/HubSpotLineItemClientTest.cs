using FakeItEasy;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.LineItem;
using Skarp.HubSpotClient.LineItem.Dto;
using Skarp.HubSpotClient.LineItem.Interfaces;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Skarp.HubSpotClient.UnitTest.LineItem
{
    public class HubSpotLineItemClientTest : UnitTestBase<HubSpotLineItemClient>
    {
        private readonly HubSpotLineItemClient _client;
        private IRapidHttpClient _mockHttpClient;
        private RequestSerializer _mockSerializer;

        public HubSpotLineItemClientTest(ITestOutputHelper output) : base(output)
        {
            _mockHttpClient = A.Fake<IRapidHttpClient>(opts => opts.Strict());

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored))
                .Returns(Task.FromResult(CreateNewEmptyOkResponse()));

            _mockSerializer = A.Fake<RequestSerializer>(opts => opts.Strict());
            A.CallTo(() => _mockSerializer.SerializeEntity(A<LineItemHubSpotEntity>.Ignored))
                .Returns("{}");

            A.CallTo(() => _mockSerializer.SerializeEntityToNameValueList<ILineItemHubSpotEntity>(A<LineItemHubSpotEntity>.Ignored))
                .Returns("[]");

            A.CallTo(() => _mockSerializer.DeserializeEntity<LineItemHubSpotEntity>(A<string>.Ignored))
                .Returns(new LineItemHubSpotEntity());

            _client = new HubSpotLineItemClient(
                _mockHttpClient,
                Logger,
                _mockSerializer,
                "https://api.hubapi.com",
                "HapiKeyFisk"
                );
        }

        private HttpResponseMessage CreateNewEmptyOkResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent("{}")
            };
            return response;
        }

        [Theory]
        [InlineData(HubSpotAction.Create, "/crm-objects/v1/objects/line_items")]
        [InlineData(HubSpotAction.CreateBatch, "/crm-objects/v1/objects/line_items/batch-create")]
        [InlineData(HubSpotAction.Get, "/crm-objects/v1/objects/line_items/:lineItemId:")]
        [InlineData(HubSpotAction.List, "/crm-objects/v1/objects/line_items/paged")]
        [InlineData(HubSpotAction.Update, "/crm-objects/v1/objects/line_items/:lineItemId:")]
        [InlineData(HubSpotAction.UpdateBatch, "/crm-objects/v1/objects/line_items/batch-update")]
        [InlineData(HubSpotAction.Delete, "/crm-objects/v1/objects/line_items/:lineItemId:")]
        [InlineData(HubSpotAction.DeleteBatch, "/crm-objects/v1/objects/line_items/batch-delete")]
        public void LineItemClient_path_resolver_works(HubSpotAction action, string expectedPath)
        {
            var resvoledPath = _client.PathResolver(new LineItemHubSpotEntity(), action);
            Assert.Equal(expectedPath, resvoledPath);
        }

        [Fact]
        public async Task LineItemClient_create_lineitem_works()
        {
            var response = await _client.CreateAsync<LineItemHubSpotEntity>(new LineItemHubSpotEntity
            {
                Name = "A new deal",
                Price = 12.50M,
                ProductId = "12345",
                Quantity = 10,
            });

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.SerializeEntityToNameValueList<ILineItemHubSpotEntity>(A<LineItemHubSpotEntity>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.DeserializeEntity<LineItemHubSpotEntity>("{}")).MustHaveHappened();
        }
    }
}
