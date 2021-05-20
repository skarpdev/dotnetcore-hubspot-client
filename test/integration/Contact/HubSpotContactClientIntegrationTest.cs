using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using integration.Contact.Dto;
using Microsoft.Extensions.Logging;
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
        private readonly string _apiKey;
        private readonly bool _isAppVeyorEnv;

        public HubSpotContactClientIntegrationTest(ITestOutputHelper output) : base(output)
        {
            _apiKey = Environment.GetEnvironmentVariable("HUBSPOT_API_KEY") ?? "demo";
            _isAppVeyorEnv = (Environment.GetEnvironmentVariable("APPVEYOR") ?? "false").Equals("true", StringComparison.InvariantCultureIgnoreCase);
;            _client = new HubSpotContactClient(
                new RealRapidHttpClient(new HttpClient()),
                base.Logger,
                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                "https://api.hubapi.com",
                _apiKey
                );
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
            if (_apiKey.Equals("demo") && _isAppVeyorEnv)
            {
                Output.WriteLine("Skipping test as the API key is incorrectly set and we're in AppVeyor");
                Assert.True(true);
                return;
            }
            
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

        [Fact]
        public async Task List_contacts_works()
        {
            var contacts =
                await _client.ListAsync<ContactListHubSpotEntity<ContactHubSpotEntityExtended>>(new ContactListRequestOptions
                {
                    NumberOfContactsToReturn = 5,
                    PropertiesToInclude = new List<string> { "lastmodifieddate" }
                });
            
            Assert.NotNull(contacts);
            Assert.NotNull(contacts.Contacts);
            Assert.NotEmpty(contacts.Contacts);
            Assert.True(contacts.Contacts.Count > 1, "contacts.Contacts.Count > 1");
        }
    }
}