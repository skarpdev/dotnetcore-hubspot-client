using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.Core.Requests;
using Xunit;
using Xunit.Abstractions;
using Skarp.HubSpotClient.Company;
using Skarp.HubSpotClient.Company.Dto;

namespace Skarp.HubSpotClient.UnitTest.Company
{
    public class HubSpotCompanyClientTest : UnitTestBase<HubSpotCompanyClient>
    {
        private readonly HubSpotCompanyClient _client;
        private IRapidHttpClient _mockHttpClient;
        private RequestSerializer _mockSerializer;

        public HubSpotCompanyClientTest(ITestOutputHelper output) : base(output)
        {
            _mockHttpClient = A.Fake<IRapidHttpClient>(opts => opts.Strict());

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored))
                .Returns(Task.FromResult(CreateNewEmptyOkResponse()));

            _mockSerializer = A.Fake<RequestSerializer>(opts => opts.Strict());
            A.CallTo(() => _mockSerializer.SerializeEntity(A<CompanyHubSpotEntity>.Ignored))
                .Returns("{}");

            A.CallTo(() => _mockSerializer.DeserializeEntity<CompanyHubSpotEntity>(A<string>.Ignored))
                .Returns(new CompanyHubSpotEntity());

            _client = new HubSpotCompanyClient(
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
        [InlineData(HubSpotAction.Create, "/companies/v2/companies")]
        [InlineData(HubSpotAction.Get, "/companies/v2/companies/:companyId:")]
        [InlineData(HubSpotAction.Update, "/companies/v2/companies/:companyId:")]
        [InlineData(HubSpotAction.Delete, "/companies/v2/companies/:companyId:")]
        public void CompanyClient_path_resolver_works(HubSpotAction action, string expetedPath)
        {
            var resvoledPath = _client.PathResolver(new CompanyHubSpotEntity(), action);
            Assert.Equal(expetedPath, resvoledPath);
        }

        [Fact]
        public async Task CompanyClient_create_contact_work()
        {
            var response = await _client.CreateAsync<CompanyHubSpotEntity>(new CompanyHubSpotEntity
            {
                Name = "A new Company",
                Description = "A new description"                
            });

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.SerializeEntity(A<IHubSpotEntity>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.DeserializeEntity<CompanyHubSpotEntity>("{}")).MustHaveHappened();
        }

    }
}
