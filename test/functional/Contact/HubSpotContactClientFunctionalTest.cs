using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Dto;
using Skarp.HubSpotClient.FunctionalTests.Mocks;
using Skarp.HubSpotClient.Requests;
using Xunit;
using Xunit.Abstractions;

namespace Skarp.HubSpotClient.FunctionalTests.Contact
{
    public class HubSpotContactClientFunctionalTest : FunctionalTestBase<HubSpotContactClient>
    {
        private readonly HubSpotContactClient _client;

        public HubSpotContactClientFunctionalTest(ITestOutputHelper output) 
        : base(output)
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
