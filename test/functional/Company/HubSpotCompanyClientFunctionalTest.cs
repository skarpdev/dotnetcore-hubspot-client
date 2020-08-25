using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Company;
using Skarp.HubSpotClient.Company.Dto;
using Skarp.HubSpotClient.Core.Requests;
using Xunit;
using Xunit.Abstractions;
using Skarp.HubSpotClient.FunctionalTests.Mocks.Company;

namespace Skarp.HubSpotClient.FunctionalTests.Company
{
    public class HubSpotCompanyClientFunctionalTest : FunctionalTestBase<HubSpotCompanyClient>
    {
        private readonly HubSpotCompanyClient _client;

        public HubSpotCompanyClientFunctionalTest(ITestOutputHelper output)
            : base(output)
        {
            var mockHttpClient = new MockRapidHttpClient()
                .AddTestCase(new CreateCompanyMockTestCase())
                .AddTestCase(new GetCompanyMockTestCase())
                .AddTestCase(new GetCompanyByIdNotFoundMockTestCase())
                .AddTestCase(new UpdateCompanyMockTestCase())
                .AddTestCase(new DeleteCompanyMockTestCase())
                .AddTestCase(new ListCompanyMockTestCase())
                .AddTestCase(new SearchByDomainMockTestCase());

            _client = new HubSpotCompanyClient(
                mockHttpClient,
                Logger,
                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                "https://api.hubapi.com/",
                "HapiKeyFisk"
            );
        }

        [Fact]
        public async Task CompanyClient_can_create_Companys()
        {
            var data = await _client.CreateAsync<CompanyHubSpotEntity>(new CompanyHubSpotEntity
            {
                Name = "A new company",
                Description = "A full description"
            });

            Assert.NotNull(data);

            // Should have replied with mocked data, so it does not really correspond to our input data, but it proves the "flow"
            Assert.Equal(10444744, data.Id);
        }

        [Fact]
        public async Task CompanyClient_can_get_Company()
        {
            const int companyId = 10444744;
            var data = await _client.GetByIdAsync<CompanyHubSpotEntity>(companyId);

            Assert.NotNull(data);
            Assert.Equal("A company name", data.Name);
            Assert.Equal("A far better description than before", data.Description);
            Assert.Equal(companyId, data.Id);
        }

        [Fact]
        public async Task CompanyClient_can_get_list_of_Companies()
        {
            var data = await _client.ListAsync<CompanyListHubSpotEntity<CompanyHubSpotEntity>>(
                new CompanyListRequestOptions
                {
                    NumberOfCompaniesToReturn = 1
                });

            Assert.NotNull(data);
            Assert.True(data.MoreResultsAvailable);
            Assert.InRange(data.ContinuationOffset, 1, long.MaxValue);
            Assert.True(data.Companies.Count == 2, "data.Companies.Count == 2");
        }

        [Fact]
        public async Task CompanyClient_returns_null_when_Company_not_found()
        {
            const int companyId = 158;
            var data = await _client.GetByIdAsync<CompanyHubSpotEntity>(companyId);

            Assert.Null(data);
        }

        [Fact]
        public async Task CompanyClient_update_Company_works()
        {
            var data = await _client.UpdateAsync<CompanyHubSpotEntity>(new CompanyHubSpotEntity
            {
                Id = 10444744,
                Name = "This is an updated company",
                Description = "This is an updated description"
            }
            );

            Assert.NotNull(data);

            // Should have replied with mocked data, so it does not really correspond to our input data, but it proves the "flow"
            Assert.Equal("This is an updated description", data.Description);
            Assert.Equal(10444744, data.Id);
        }

        [Fact]
        public async Task CompanyClient_delete_Company_works()
        {
            await _client.DeleteAsync(10444744);
        }

        [Fact]
        public async Task CompanyClient_search_by_domain_works()
        {
            var request = new CompanySearchByDomain();

            var response = await _client.GetByEmailAsync<CompanySearchResultEntity<CompanyHubSpotEntity>>("mr@miaki.io", request);

            Assert.NotNull(response);
            Assert.NotEmpty(response.Results);
        }
    }
}