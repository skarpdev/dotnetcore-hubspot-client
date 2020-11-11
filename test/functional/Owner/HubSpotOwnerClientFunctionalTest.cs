using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Core.Requests;
using Xunit;
using Xunit.Abstractions;
using Skarp.HubSpotClient.FunctionalTests.Mocks.Owner;
using Skarp.HubSpotClient.Owner;
using Skarp.HubSpotClient.Owner.Dto;
using System.Linq;

namespace Skarp.HubSpotClient.FunctionalTests.Owner
{
    public class HubSpotOwnerClientFunctionalTest : FunctionalTestBase<HubSpotOwnerClient>
    {
        private readonly HubSpotOwnerClient _client;

        public HubSpotOwnerClientFunctionalTest(ITestOutputHelper output)
            : base(output)
        {
            var mockHttpClient = new MockRapidHttpClient()
                .AddTestCase(new GetOwnerMockTestCase())
                .AddTestCase(new GetOwnerByIdNotFoundMockTestCase())
                .AddTestCase(new ListOwnerMockTestCase());

            _client = new HubSpotOwnerClient(
                mockHttpClient,
                Logger,
                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                "https://api.hubapi.com/",
                "HapiKeyFisk"
            );
        }

        [Fact]
        public async Task OwnerClient_can_get_owner_by_id_long()
        {
            const int ownerId = 64;
            var data = await _client.GetByIdAsync<OwnerHubSpotEntity>(ownerId);

            Assert.NotNull(data);
            Assert.Equal("An owner first name", data.FirstName);
            Assert.Equal("An owner last name", data.LastName);
            Assert.Equal("owner@company.com", data.Email);
            Assert.Equal(ownerId, data.Id);
        }

        [Fact]
        public async Task OwnerClient_can_get_owner_by_id_string()
        {
            const string ownerId = "64";
            var data = await _client.GetByIdAsync<OwnerHubSpotEntity>(ownerId);

            Assert.NotNull(data);
            Assert.Equal("An owner first name", data.FirstName);
            Assert.Equal("An owner last name", data.LastName);
            Assert.Equal("owner@company.com", data.Email);
            Assert.Equal(ownerId, data.Id.ToString());
        }

        [Fact]
        public async Task OwnerClient_can_get_list_of_owners()
        {
            var data = await _client.ListAsync<OwnerHubSpotEntity>();

            Assert.NotNull(data);
            Assert.True(data.Count() == 3, "data.Count() == 3");
            Assert.False(data.Any(owner => string.IsNullOrEmpty(owner.FirstName)), "All owners should have data");
        }

        [Fact]
        public async Task OwnerClient_returns_null_when_owner_not_found()
        {
            const int ownerId = 158;
            var data = await _client.GetByIdAsync<OwnerHubSpotEntity>(ownerId);

            Assert.Null(data);
        }

        [Fact]
        public async Task OwnerClient_list_by_email_works()
        {
            var options = new OwnerListRequestOptions
            {
                Email = "owner@company.com"
            };

            var response = await _client.ListAsync<OwnerHubSpotEntity>(options);

            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.False(response.Any(owner => string.IsNullOrEmpty(owner.FirstName)), "All owners should have data");
        }

        [Fact]
        public async Task OwnerClient_list_with_include_inactive_works()
        {
            var options = new OwnerListRequestOptions
            {
                IncludeInactive = true
            };

            var response = await _client.ListAsync<OwnerHubSpotEntity>(options);

            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.False(response.Any(owner => string.IsNullOrEmpty(owner.FirstName)), "All owners should have data");
        }
    }
}
