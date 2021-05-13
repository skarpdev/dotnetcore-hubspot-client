using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Company;
using Skarp.HubSpotClient.Company.Dto.Properties;
using Skarp.HubSpotClient.Core.Requests;
using Xunit;
using Xunit.Abstractions;

namespace integration.Company
{
    public class HubSpotCompanyClientIntegrationTest : IntegrationTestBase<HubSpotCompanyClient>
    {
        private readonly HubSpotCompanyClient _client;
        private readonly string _apiKey;
        private readonly bool _isAppVeyorEnv;

        public HubSpotCompanyClientIntegrationTest(ITestOutputHelper output) : base(output)
        {
            _apiKey = Environment.GetEnvironmentVariable("HUBSPOT_API_KEY") ?? "demo";
            _isAppVeyorEnv = (Environment.GetEnvironmentVariable("APPVEYOR") ?? "false").Equals("true", StringComparison.InvariantCultureIgnoreCase);
;            _client = new HubSpotCompanyClient(
                new RealRapidHttpClient(new HttpClient()),
                base.Logger,
                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                "https://api.hubapi.com",
                _apiKey
                );
        }

        [Fact]
        public async Task GetProperties()
        {
            if (_isAppVeyorEnv)
            {
                Output.WriteLine("Skipping test as we're in AppVeyor, demo account does return 137 results");
                Assert.True(true);
                return;
            }

            var properties = await _client.GetPropertiesAsync<CompanyPropertyListHubSpotEntity<CompanyPropertyHubSpotEntity>>();
            
            Assert.NotNull(properties);
            Assert.NotEmpty(properties);
            Assert.True(properties.Count > 1, "properties.Count > 1");
        }
    }
}