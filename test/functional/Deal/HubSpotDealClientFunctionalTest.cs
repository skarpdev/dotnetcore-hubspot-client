using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Deal;
using Skarp.HubSpotClient.Deal.Dto;
using Skarp.HubSpotClient.Core.Requests;
using Xunit;
using Xunit.Abstractions;
using Skarp.HubSpotClient.FunctionalTests.Mocks.Deal;

namespace Skarp.HubSpotClient.FunctionalTests.Deal
{
    public class HubSpotDealClientFunctionalTest : FunctionalTestBase<HubSpotDealClient>
    {
        private readonly HubSpotDealClient _client;

        public HubSpotDealClientFunctionalTest(ITestOutputHelper output)
            : base(output)
        {
            var mockHttpClient = new MockRapidHttpClient()
                .AddTestCase(new CreateDealMockTestCase())
                .AddTestCase(new GetDealMockTestCase())
                .AddTestCase(new GetDealByIdNotFoundMockTestCase())
                .AddTestCase(new UpdateDealMockTestCase())
                .AddTestCase(new DeleteDealMockTestCase());

            _client = new HubSpotDealClient(
                mockHttpClient,
                Logger,
                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                "https://api.hubapi.com/",
                "HapiKeyFisk"
            );
        }

        [Fact]
        public async Task DealClient_can_create_Deals()
        {
            var data = await _client.CreateAsync<DealHubSpotEntity>(new DealHubSpotEntity
            {
                Name = "A new deal",
                Pipeline = "default",
                Amount = "60000",
                DealType = "newbusiness"
            });

            Assert.NotNull(data);

            // Should have replied with mocked data, so it does not really correspond to our input data, but it proves the "flow"
            Assert.Equal(151088, data.Id);
        }

        [Fact]
        public async Task DealClient_can_get_Deal()
        {
            const int dealId = 151088;
            var data = await _client.GetByIdAsync<DealHubSpotEntity>(dealId);

            Assert.NotNull(data);
            Assert.Equal("This is a Deal", data.Name);
            Assert.Equal("560", data.Amount);
            Assert.Equal(dealId, data.Id);
        }


        [Fact]
        public async Task DealClient_returns_null_when_Deal_not_found()
        {
            const int dealId = 158;
            var data = await _client.GetByIdAsync<DealHubSpotEntity>(dealId);

            Assert.Null(data);
        }

        [Fact]
        public async Task DealClient_update_Deal_works()
        {
            var data = await _client.UpdateAsync<DealHubSpotEntity>(new DealHubSpotEntity
            {
                Id = 151088,
                Name = "This is an updated deal",
                Amount = "560"
            }
            );

            Assert.NotNull(data);

            // Should have replied with mocked data, so it does not really correspond to our input data, but it proves the "flow"
            Assert.Equal("560", data.Amount);
            Assert.Equal(151088, data.Id);
        }

        [Fact]
        public async Task DealClient_delete_Deal_works()
        {
            await _client.DeleteAsync(151088);
        }
    }
}