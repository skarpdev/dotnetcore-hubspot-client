using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Core.Requests;
using Xunit;
using Xunit.Abstractions;
using Skarp.HubSpotClient.FunctionalTests.Mocks.LineItem;
using Skarp.HubSpotClient.LineItem;
using Skarp.HubSpotClient.LineItem.Dto;
using Skarp.HubSpotClient.LineItem.Interfaces;
using System.Linq;

namespace Skarp.HubSpotClient.FunctionalTests.LineItem
{
    public class HubSpotLineItemClientFunctionalTest : FunctionalTestBase<HubSpotLineItemClient>
    {
        private readonly HubSpotLineItemClient _client;

        public HubSpotLineItemClientFunctionalTest(ITestOutputHelper output)
            : base(output)
        {
            var mockHttpClient = new MockRapidHttpClient()
                .AddTestCase(new CreateLineItemMockTestCase())
                .AddTestCase(new CreateBatchLineItemMockTestCase())
                .AddTestCase(new GetLineItemMockTestCase())
                .AddTestCase(new GetLineItemNotFoundMockTestCase())
                .AddTestCase(new ListLineItemMockTestCase())
                .AddTestCase(new UpdateLineItemMockTestCase())
                .AddTestCase(new UpdateBatchLineItemMockTestCase())
                .AddTestCase(new DeleteLineItemMockTestCase())
                .AddTestCase(new DeleteBatchLineItemMockTestCase());

            _client = new HubSpotLineItemClient(
                mockHttpClient,
                Logger,
                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                "https://api.hubapi.com/",
                "HapiKeyFisk"
            );
        }

        [Fact]
        public async Task LineItemClient_can_create_LineItems()
        {
            var data = await _client.CreateAsync<LineItemHubSpotEntity>(new LineItemHubSpotEntity
            {
                Name = "A new line item",
                Price = 12.50M,
                ProductId = "12345",
                Quantity = 10,
            });

            Assert.NotNull(data);

            // Should have replied with mocked data, so it does not really correspond to our input data, but it proves the "flow"
            Assert.Equal(9867220, data.Id);
        }

        [Fact]
        public async Task LineItemClient_batch_create_LineItem_works()
        {
            var data = await _client.CreateBatchAsync<LineItemHubSpotEntity>(new[] {
                new LineItemHubSpotEntity
                {
                    Name = "This is a new LineItem",
                    Quantity = 666,
                    Price = 9.99M,
                    ProductId = "12345"
                },
                new LineItemHubSpotEntity
                {
                    Name = "This is another new LineItem",
                    Quantity = 999,
                    Price = 12.45M,
                    ProductId = "67890"
                }
            });

            Assert.Equal(2, data.Count());
            foreach (var item in data)
            {
                Assert.NotEqual(0, item.Id);
                Assert.NotEqual(0, item.Price);
                Assert.NotEqual(0, item.Quantity);
                Assert.NotNull(item.Name);
                Assert.NotNull(item.ProductId);
            }
        }

        [Fact]
        public async Task LineItemClient_can_get_LineItem()
        {
            const int lineItemId = 9867220;

            var options = new LineItemGetRequestOptions
            {
                IncludeDeletes = true
            };
            options.PropertiesToInclude.Add("my_custom_property");

            var data = await _client.GetByIdAsync<LineItemHubSpotEntity>(lineItemId, options);

            Assert.NotNull(data);
            Assert.Equal("This is a LineItem", data.Name);
            Assert.Equal(9.50M, data.Price);
            Assert.Equal(50, data.Quantity);
            Assert.Equal(lineItemId, data.Id);
        }

        [Fact]
        public async Task LineItemClient_can_list_LineItem()
        {
            var options = new LineItemListRequestOptions
            {
                Offset = 9867220
            };
            options.PropertiesToInclude.Add("my_custom_property");

            var data = await _client.ListAsync<LineItemListHubSpotEntity<LineItemHubSpotEntity>>(options);

            Assert.NotNull(data);
            Assert.Equal(9867220, data.ContinuationOffset);
            Assert.True(data.MoreResultsAvailable);
            Assert.Equal(2, data.LineItems.Count());
            foreach (var item in data.LineItems)
            {
                Assert.NotNull(item.ProductId); // sample response only has product Id's
            }
        }

        [Fact]
        public async Task LineItemClient_returns_null_when_LineItem_not_found()
        {
            const int lineItemId = 158;
            var data = await _client.GetByIdAsync<LineItemHubSpotEntity>(lineItemId);

            Assert.Null(data);
        }

        [Fact]
        public async Task LineItemClient_update_LineItem_works()
        {
            var data = await _client.UpdateAsync<LineItemHubSpotEntity>(new LineItemHubSpotEntity
            {
                Id = 9867220,
                Name = "This is an updated LineItem",
                Quantity = 666,
                Price = 9.99M,
                ProductId = "12345"
            });

            Assert.NotNull(data);

            // Should have replied with mocked data, so it does not really correspond to our input data, but it proves the "flow"
            Assert.NotNull(data);
            Assert.Equal("This is a LineItem", data.Name);
            Assert.Equal(9.50M, data.Price);
            Assert.Equal(50, data.Quantity);
            Assert.Equal(9867220, data.Id);
        }

        [Fact]
        public async Task LineItemClient_delete_LineItem_works()
        {
            await _client.DeleteAsync(9867220);
        }

        [Fact]
        public async Task LineItemClient_batch_delete_LineItem_works()
        {
            var request = new ListOfLineItemIds();

            request.Ids.Add(9867220);
            request.Ids.Add(9867221);

            await _client.DeleteBatchAsync(request);
        }

        [Fact]
        public async Task LineItemClient_batch_update_LineItem_works()
        {
            var data = await _client.UpdateBatchAsync<LineItemHubSpotEntity>(new[] {
                new LineItemHubSpotEntity
                {
                    Id = 9867220,
                    Name = "This is an updated LineItem",
                    Quantity = 666,
                    Price = 9.99M,
                    ProductId = "12345"
                },
                new LineItemHubSpotEntity
                {
                    Id = 9867221,
                    Name = "This is another updated LineItem",
                    Quantity = 999,
                    Price = 12.45M,
                    ProductId = "67890"
                }
            });

            Assert.Equal(2, data.Count());
            foreach (var item in data)
            {
                Assert.NotEqual(0, item.Id);
                Assert.NotEqual(0, item.Price);
                Assert.NotEqual(0, item.Quantity);
                Assert.NotNull(item.Name);
                Assert.NotNull(item.ProductId);
            }
        }
    }
}