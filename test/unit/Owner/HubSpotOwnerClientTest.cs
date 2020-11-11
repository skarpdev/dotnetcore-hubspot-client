using FakeItEasy;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.Owner;
using Skarp.HubSpotClient.Owner.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Skarp.HubSpotClient.UnitTest.Owner
{
    public class HubSpotOwnerClientTest : UnitTestBase<HubSpotOwnerClient>
    {
        private readonly HubSpotOwnerClient _client;
        private readonly IRapidHttpClient _mockHttpClient;
        private readonly RequestSerializer _mockSerializer;

        public HubSpotOwnerClientTest(ITestOutputHelper output) : base(output)
        {
            _mockHttpClient = A.Fake<IRapidHttpClient>(opts => opts.Strict());

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored))
                .Returns(Task.FromResult(CreateNewEmptyOkResponse()));

            _mockSerializer = A.Fake<RequestSerializer>(opts => opts.Strict());
            A.CallTo(() => _mockSerializer.SerializeEntity(A<OwnerHubSpotEntity>.Ignored))
                .Returns("{}");

            A.CallTo(() => _mockSerializer.DeserializeGenericEntity<OwnerHubSpotEntity>(A<string>.Ignored))
                .Returns(new OwnerHubSpotEntity());

            A.CallTo(() => _mockSerializer.DeserializeGenericEntity<IEnumerable<OwnerHubSpotEntity>>(A<string>.Ignored))
                .Returns(Enumerable.Empty<OwnerHubSpotEntity>());

            A.CallTo(() => _mockSerializer.SerializeEntities(A<List<OwnerHubSpotEntity>>.Ignored))
                .Returns("{}");

            _client = new HubSpotOwnerClient(
                _mockHttpClient,
                Logger,
                _mockSerializer,
                "https://api.hubapi.com",
                "HapiKeyFisk"
                );
        }

        private HttpResponseMessage CreateNewEmptyOkResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent("{}")
            };
            return response;
        }

        [Theory]
        [InlineData(HubSpotAction.List, "/owners/v2/owners")]
        [InlineData(HubSpotAction.Get, "/owners/v2/owners/:ownerId:")]
        public void OwnerClient_path_resolver_works(HubSpotAction action, string expectedPath)
        {
            var resvoledPath = _client.PathResolver(new OwnerHubSpotEntity(), action);
            Assert.Equal(expectedPath, resvoledPath);
        }

        [Fact]
        public async Task OwnerClient_get_owner_by_id_long_works()
        {
            var response = await _client.GetByIdAsync<OwnerHubSpotEntity>(64);

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.DeserializeGenericEntity<OwnerHubSpotEntity>("{}")).MustHaveHappened();
        }

        [Fact]
        public async Task OwnerClient_get_owner_by_id_string_works()
        {
            var response = await _client.GetByIdAsync<OwnerHubSpotEntity>("64");

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.DeserializeGenericEntity<OwnerHubSpotEntity>("{}")).MustHaveHappened();
        }

        [Fact]
        public async Task OwnerClient_list_owners_works()
        {
            var response = await _client.ListAsync<OwnerHubSpotEntity>(
                new OwnerListRequestOptions
                {
                    IncludeInactive = true,
                    Email = "owner@company.com"
                });

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.DeserializeGenericEntity<IEnumerable<OwnerHubSpotEntity>>("{}")).MustHaveHappened();
        }
    }
}
