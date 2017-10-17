using System;
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

        [Fact]
        public async Task Create_contact_and_get_works()
        {
            var contact = new ContactHubSpotEntity
            {
                Address = "Som street 42",
                City = "Appleseed",
                Company = "Damage Inc.",
                Email = $"{Guid.NewGuid():N}@skarp.dk",
                FirstName = "Mr",
                Lastname = "Tester",
                Phone = "+45 12345678",
                State = "",
                ZipCode = "2300"
            };
            var created = await _client.CreateAsync<ContactHubSpotEntity>(contact);
            
            Assert.NotNull(created.Id);

            var retrieved = await _client.GetByIdAsync<ContactHubSpotEntity>(created.Id.Value);
            
            Assert.NotNull(retrieved);
            Assert.Equal("2300", retrieved.ZipCode);
        }
    }
}