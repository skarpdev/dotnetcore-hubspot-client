using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.Deal;
using Skarp.HubSpotClient.Deal.Dto;
using Xunit;
using Xunit.Abstractions;

namespace Skarp.HubSpotClient.UnitTest.Deal
{
    public class HubSpotDealClientTest : UnitTestBase<HubSpotDealClient>
    {
        private readonly HubSpotDealClient _client;
        private IRapidHttpClient _mockHttpClient;
        private RequestSerializer _mockSerializer;

        public HubSpotDealClientTest(ITestOutputHelper output) : base(output)
        {
            _mockHttpClient = A.Fake<IRapidHttpClient>(opts => opts.Strict());

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored))
                .Returns(Task.FromResult(CreateNewEmptyOkResponse()));

            _mockSerializer = A.Fake<RequestSerializer>(opts => opts.Strict());
            A.CallTo(() => _mockSerializer.SerializeEntity(A<DealHubSpotEntity>.Ignored))
                .Returns("{}");

            A.CallTo(() => _mockSerializer.DeserializeEntity<DealHubSpotEntity>(A<string>.Ignored))
                .Returns(new DealHubSpotEntity());

            A.CallTo(() => _mockSerializer.DeserializeListEntity<DealListHubSpotEntity<DealHubSpotEntity>>(A<string>.Ignored))
                .Returns(new DealListHubSpotEntity<DealHubSpotEntity>());

            _client = new HubSpotDealClient(
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
        [InlineData(HubSpotAction.Create, "/deals/v1/deal")]
        [InlineData(HubSpotAction.Get, "/deals/v1/deal/:dealId:")]
        [InlineData(HubSpotAction.Update, "/deals/v1/deal/:dealId:")]
        [InlineData(HubSpotAction.Delete, "/deals/v1/deal/:dealId:")]
        [InlineData(HubSpotAction.List, "/deals/v1/deal/paged")]
        public void DealClient_path_resolver_works(HubSpotAction action, string expetedPath)
        {
            var resvoledPath = _client.PathResolver(new DealHubSpotEntity(), action);
            Assert.Equal(expetedPath, resvoledPath);
        }

        [Fact]
        public async Task DealClient_create_contact_work()
        {
            var response = await _client.CreateAsync<DealHubSpotEntity>(new DealHubSpotEntity
            {
                Name = "A new deal",
                Pipeline = "default",
                Amount = 60000,
                DealType = "newbusiness"
            });

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.SerializeEntity(A<IHubSpotEntity>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.DeserializeEntity<DealHubSpotEntity>("{}")).MustHaveHappened();
        }

        [Fact]
        public async Task DealClient_list_work()
        {
            var response = await _client.ListAsync<DealListHubSpotEntity<DealHubSpotEntity>>();

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            //A.CallTo(() => _mockSerializer.SerializeEntity(A<IHubSpotEntity>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.DeserializeListEntity<DealListHubSpotEntity<DealHubSpotEntity>>("{}")).MustHaveHappened();
        }
    }
}
