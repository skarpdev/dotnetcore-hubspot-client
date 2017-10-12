using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using RapidCore.Network;
using Skarp.HubSpotClient.Contact;
using Skarp.HubSpotClient.Contact.Dto;
using Skarp.HubSpotClient.Core.Requests;
using Xunit;
using Xunit.Abstractions;

namespace integration.Contact
{
    public class HubSpotContactClientIntegrationTest : IntegrationTestBase<HubSpotContactClient>
    {
        private readonly HubSpotContactClient _client;

        public HubSpotContactClientIntegrationTest(ITestOutputHelper output) : base(output)
        {
            _client = new HubSpotContactClient(
                new RealRapidHttpClient(new HttpClient()),
                base.Logger,
                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                "https://api.hubapi.com",
                "demo");
        }

        [Fact]
        public async Task Get_non_existing_contact_works()
        {
            var contact = await _client.GetByEmailAsync<ContactHubSpotEntity>("iamnothere@skarp.dk");
            Assert.Null(contact);
        }
    }
}