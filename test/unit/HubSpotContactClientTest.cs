using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Dto;
using Skarp.HubSpotClient.Requests;
using Xunit;
using Xunit.Abstractions;

namespace Skarp.HubSpotClient.UnitTest
{
    public class HubSpotContactClientTest : UnitTestBase<HubSpotContactClient>
    {
        private HubSpotContactClient _client;

        public HubSpotContactClientTest(ITestOutputHelper output) : base(output)
        {
            var mockHttpClient = new MockRapidHttpClient()
                .AddTestCase(new CreateContactMockTestCase());
            
            _client = new HubSpotContactClient(
                mockHttpClient, 
                Logger,
                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())), 
                "https://api.hubapi.com/",
                "HapiKeyFisk"
                );
        }

        [Theory]
        [InlineData(HubSpotAction.Create, "/contacts/v1/contact")]
        [InlineData(HubSpotAction.Get, "/contacts/v1/contact/vid/:contactId:/profile")]
        [InlineData(HubSpotAction.List, "/contacts/v1/lists/all/contacts/all")]
        [InlineData(HubSpotAction.Update, "/contacts/v1/contact/vid/:contactId:/profile")]
        [InlineData(HubSpotAction.Delete, "/contacts/v1/contact/vid/:contactId:")]
        public void ContactClient_path_resolver_works(HubSpotAction action, string expetedPath)
        {
            var resvoledPath = _client.PathResolver(new ContactHubSpotEntity(), action);
            Assert.Equal(expetedPath, resvoledPath);
        }

        [Fact]
        public async Task ContactClient_can_create_contacts()
        {
            var data = await _client.CreateAsync<ContactHubSpotEntity>(new ContactHubSpotEntity
            {
                FirstName = "Mr",
                Lastname = "Test miaki",
                Address = "Æblevej 42",
                City = "Copenhagen",
                ZipCode = "2300",
                Company = "Acme inc",
                Email = "that@email.com",

            });
            
            Assert.NotNull(data);
            
            // Should have replied with mocked data, so it does not really correspond to our input data, but it proves the "flow"
            Assert.Equal(61574, data.Vid);
        }
    }
}
