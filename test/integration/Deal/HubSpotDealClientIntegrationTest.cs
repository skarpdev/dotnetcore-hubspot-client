using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Common.Dto.Properties;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.Deal;
using Skarp.HubSpotClient.Deal.Dto;
using Xunit;
using Xunit.Abstractions;

namespace integration.Deal
{
    public class HubSpotDealClientIntegrationTest : IntegrationTestBase<HubSpotDealClient>
    {
        private readonly HubSpotDealClient _client;
        private readonly string _apiKey;
        private readonly bool _isAppVeyorEnv;

        public HubSpotDealClientIntegrationTest(ITestOutputHelper output) : base(output)
        {
            _apiKey = Environment.GetEnvironmentVariable("HUBSPOT_API_KEY") ?? "demo";
            _isAppVeyorEnv = (Environment.GetEnvironmentVariable("APPVEYOR") ?? "false").Equals("true", StringComparison.InvariantCultureIgnoreCase);
;            _client = new HubSpotDealClient(
                new RealRapidHttpClient(new HttpClient()),
                base.Logger,
                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                "https://api.hubapi.com",
                _apiKey
                );
        }

        [Fact]
        public async Task Create_deal_no_associations()
        {
            if (_apiKey.Equals("demo") && _isAppVeyorEnv)
            {
                Output.WriteLine("Skipping test as the API key is incorrectly set and we're in AppVeyor");
                Assert.True(true);
                return;
            }

            var deal = new DealHubSpotEntity
            {
                Name = "Skarp Demo Deal Create",
                Amount = 1250
            };
            var created = await _client.CreateAsync<DealHubSpotEntity>(deal);

            Assert.NotNull(created.Id);

            var retrieved = await _client.GetByIdAsync<DealHubSpotEntity>(created.Id.Value);

            Assert.NotNull(retrieved);
            Assert.Equal(1250, retrieved.Amount);
        }

        [Fact]
        public async Task List()
        {
            if (_isAppVeyorEnv)
            {
                Output.WriteLine("Skipping test as we're in AppVeyor, demo account does return 3 results");
                Assert.True(true);
                return;
            }

            var deals =
                await _client.ListAsync<DealListHubSpotEntity<DealHubSpotEntity>>(new DealListRequestOptions
                {
                    PropertiesToInclude = new List<string> { "dealname", "amount", "hubspot_owner_id" },
                    NumberOfDealsToReturn = 2
                });
            
            Assert.NotNull(deals);
            Assert.NotNull(deals.Deals);
            Assert.NotEmpty(deals.Deals);
            Assert.True(deals.Deals.Count > 1, "deals.Deals.Count > 1");
        }

        [Fact]
        public async Task ListAll()
        {
            if (_isAppVeyorEnv)
            {
                Output.WriteLine("Skipping test as we're in AppVeyor, demo account does return 3 results");
                Assert.True(true);
                return;
            }

            var deals = new List<DealHubSpotEntity>();
            var moreResults = true;
            long offset = 0;

            while (moreResults)
            {
                var pagedDeals = await _client.ListAsync<DealListHubSpotEntity<DealHubSpotEntity>>(new DealListRequestOptions
                {
                    PropertiesToInclude = new List<string>
                    {
                        "dealname",
                        "dealstage",
                        "pipeline",
                        "hubspot_owner_id",
                        "closedate",
                        "amount",
                        "dealtype"
                    },
                    NumberOfDealsToReturn = 150,
                    DealOffset = offset
                });

                deals.AddRange(pagedDeals.Deals);

                moreResults = pagedDeals.MoreResultsAvailable;
                if (moreResults)
                    offset = pagedDeals.ContinuationOffset;
            }

            Assert.NotNull(deals);
            Assert.NotEmpty(deals);
            Assert.True(deals.Count > 1, "deals.Count > 1");
        }


        [Fact]
        public async Task GetProperties()
        {
            if (_isAppVeyorEnv)
            {
                Output.WriteLine("Skipping test as we're in AppVeyor, demo account does return 117 results");
                Assert.True(true);
                return;
            }

            var properties = await _client.GetPropertiesAsync<PropertyListHubSpotEntity<DealPropertyHubSpotEntity>>();

            Assert.NotNull(properties);
            Assert.NotEmpty(properties);
            Assert.True(properties.Count > 1, "properties.Count > 1");
        }
    }
}