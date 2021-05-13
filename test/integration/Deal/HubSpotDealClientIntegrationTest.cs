using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
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
                    PropertiesToInclude = new List<string> { "dealname", "amount" },
                    NumberOfDealsToReturn = 2
                });
            
            Assert.NotNull(deals);
            Assert.NotNull(deals.Deals);
            Assert.NotEmpty(deals.Deals);
            Assert.True(deals.Deals.Count > 1, "contacts.Deals.Count > 1");
        }
    }
}