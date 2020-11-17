using RapidCore.Network;
using Microsoft.Extensions.Logging;
using Skarp.HubSpotClient.Associations;
using Skarp.HubSpotClient.Core.Requests;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit;
using System.Threading.Tasks;
using Skarp.HubSpotClient.Associations.Dto;
using Skarp.HubSpotClient.FunctionalTests.Mocks.Association;
using Skarp.HubSpotClient.Core;

namespace Skarp.HubSpotClient.FunctionalTests.Association
{
    public class HubSpotAssociationClientFunctionalTest : FunctionalTestBase<HubSpotAssociationsClient>
    {
        private readonly HubSpotAssociationsClient _client;

        public HubSpotAssociationClientFunctionalTest(ITestOutputHelper output)
            : base(output)
        {
            var mockHttpClient = new MockRapidHttpClient()
                .AddTestCase(new CreateAssociationMockTestCase())
                .AddTestCase(new CreateBatchAssociationMockTestCase())
                .AddTestCase(new DeleteAssociationMockTestCase())
                .AddTestCase(new DeleteBatchAssociationMockTestCase())
                .AddTestCase(new ListAssociationMockTestCase());

            _client = new HubSpotAssociationsClient(
                mockHttpClient,
                Logger,
                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                "https://api.hubapi.com/",
                "HapiKeyFisk"
            );
        }

        [Fact]
        public async Task CompanyClient_can_get_list_of_Associations()
        {
            var data = await _client.GetListByIdAsync(
                10444744,
                Core.HubSpotAssociationDefinitions.CompanyToContact,
                new AssociationListRequestOptions
                {
                    NumberOfAssociationsToReturn = 1
                });

            Assert.NotNull(data);
            Assert.True(data.MoreResultsAvailable);
            Assert.InRange(data.ContinuationOffset, 1, long.MaxValue);
            Assert.True(data.Results.Count == 2, "data.Results.Count == 2");
        }

        [Fact]
        public async Task AssociationClient_can_create_Association()
        {
            var result = await _client.Create(new AssociationHubSpotEntity
            {
                FromObjectId = 10444744,
                ToObjectId = 259674,
                DefinitionId = (int)HubSpotAssociationDefinitions.CompanyToContact
            });

            Assert.True(result);
        }

        [Fact]
        public async Task AssociationClient_can_create_batch_Association()
        {
            var entities = new List<AssociationHubSpotEntity>
            {
                new AssociationHubSpotEntity
            {
                FromObjectId = 10444744,
                ToObjectId = 259674,
                DefinitionId = (int)HubSpotAssociationDefinitions.CompanyToContact
            },
                    new AssociationHubSpotEntity
            {
                FromObjectId = 10444744,
                ToObjectId = 259727,
                DefinitionId = (int)HubSpotAssociationDefinitions.CompanyToContact
            }
            };
            var result = await _client.CreateBatch(entities);

            Assert.True(result);
        }

        [Fact]
        public async Task AssociationClient_fail_create_batch_Association()
        {
            var mockHttpClient = new MockRapidHttpClient()
                .AddTestCase(new CreateBatchAssociationMockFailTestCase());
            var localClient = new HubSpotAssociationsClient(
                mockHttpClient,
                Logger,
                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                "https://api.hubapi.com/",
                "HapiKeyFisk"
            );


            await Assert.ThrowsAsync<HubSpotException>(async () =>
             {
                 var entities = new List<AssociationHubSpotEntity>
             {
                new AssociationHubSpotEntity
            {
                FromObjectId = 10444744,
                ToObjectId = 259674,
                DefinitionId = (int)HubSpotAssociationDefinitions.CompanyToContact
            },
                    new AssociationHubSpotEntity
            {
                FromObjectId = 10444744,
                ToObjectId = 259727,
                DefinitionId = (int)HubSpotAssociationDefinitions.CompanyToContact
            }
             };
                 _ = await localClient.CreateBatch(entities);
             });

        }

        [Fact]
        public async Task ContactClient_delete_contact_works()
        {
            var result = await _client.Delete(new AssociationHubSpotEntity
            {
                FromObjectId = 10444744,
                ToObjectId = 259674,
                DefinitionId = (int)HubSpotAssociationDefinitions.CompanyToContact
            });

            Assert.True(result);
        }

        [Fact]
        public async Task AssociationClient_can_delete_batch_Association()
        {
            var entities = new List<AssociationHubSpotEntity>
            {
                new AssociationHubSpotEntity
            {
                FromObjectId = 10444744,
                ToObjectId = 259674,
                DefinitionId = (int)HubSpotAssociationDefinitions.CompanyToContact
            },
                    new AssociationHubSpotEntity
            {
                FromObjectId = 10444744,
                ToObjectId = 259727,
                DefinitionId = (int)HubSpotAssociationDefinitions.CompanyToContact
            }
            };
            var result = await _client.DeleteBatch(entities);

            Assert.True(result);
        }
    }
}
