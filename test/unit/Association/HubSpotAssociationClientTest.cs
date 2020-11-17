using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Requests;
using Xunit;
using Xunit.Abstractions;
using Skarp.HubSpotClient.Associations;
using Skarp.HubSpotClient.Associations.Dto;
using System.Collections.Generic;

namespace Skarp.HubSpotClient.UnitTest.Association
{
    public class HubSpotAssociationClientTest : UnitTestBase<HubSpotAssociationsClient>
    {
        private readonly HubSpotAssociationsClient _client;
        private IRapidHttpClient _mockHttpClient;
        private RequestSerializer _mockSerializer;

        public HubSpotAssociationClientTest(ITestOutputHelper output) : base(output)
        {
            _mockHttpClient = A.Fake<IRapidHttpClient>(opts => opts.Strict());

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored))
                .Returns(Task.FromResult(CreateNewEmptyOkResponse()));

            _mockSerializer = A.Fake<RequestSerializer>(opts => opts.Strict());
            A.CallTo(() => _mockSerializer.SerializeEntity(A<AssociationHubSpotEntity>.Ignored))
                .Returns("{}");

            A.CallTo(() => _mockSerializer.DeserializeGenericEntity<AssociationHubSpotEntity>(A<string>.Ignored))
                .Returns(new AssociationHubSpotEntity());

            A.CallTo(() => _mockSerializer.DeserializeGenericEntity<AssociationListHubSpotEntity<long>>(A<string>.Ignored))
                .Returns(new AssociationListHubSpotEntity<long>());
            
            A.CallTo(() => _mockSerializer.SerializeEntities(A<List<AssociationHubSpotEntity>>.Ignored))
                .Returns("{}");

            _client = new HubSpotAssociationsClient(
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
        [InlineData(HubSpotAction.List, "/crm-associations/v1/associations/:fromObjectId:/HUBSPOT_DEFINED/:definitionId:")]
        [InlineData(HubSpotAction.Create, "/crm-associations/v1/associations")]
        [InlineData(HubSpotAction.CreateBatch, "/crm-associations/v1/associations/create-batch")]
        [InlineData(HubSpotAction.Delete, "/crm-associations/v1/associations/delete")]
        [InlineData(HubSpotAction.DeleteBatch, "/crm-associations/v1/associations/delete-batch")]
        public void AssociationClient_path_resolver_works(HubSpotAction action, string expetedPath)
        {
            var resvoledPath = _client.PathResolver(new AssociationHubSpotEntity(), action);
            Assert.Equal(expetedPath, resvoledPath);
        }

        [Fact]
        public async Task AssociationClient_create_association_work()
        {
            var response = await _client.Create(new AssociationHubSpotEntity
            {
                FromObjectId = 10444744,
                ToObjectId = 259674,
                DefinitionId = (int)HubSpotAssociationDefinitions.CompanyToContact
            });

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.SerializeEntity(A<AssociationHubSpotEntity>.Ignored)).MustHaveHappened();
        }

        [Fact]
        public async Task AssociationClient_create_batch_association_work()
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

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.SerializeEntities(A<List<AssociationHubSpotEntity>>.Ignored)).MustHaveHappened();
        }

        [Fact]
        public async Task AssociationClient_delete_association_work()
        {
            var response = await _client.Delete(new AssociationHubSpotEntity
            {
                FromObjectId = 10444744,
                ToObjectId = 259674,
                DefinitionId = (int)HubSpotAssociationDefinitions.CompanyToContact
            });

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.SerializeEntity(A<AssociationHubSpotEntity>.Ignored)).MustHaveHappened();
        }

        [Fact]
        public async Task AssociationClient_delete_batch_association_work()
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

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.SerializeEntities(A<List<AssociationHubSpotEntity>>.Ignored)).MustHaveHappened();
        }

        [Fact]
        public async Task AssociationClient_list_companies_work()
        {
            var response = await _client.GetListByIdAsync(
                10444744,
                HubSpotAssociationDefinitions.CompanyToContact,
                new AssociationListRequestOptions
                {
                    NumberOfAssociationsToReturn = 1
                });

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.DeserializeGenericEntity<AssociationListHubSpotEntity<long>>("{}")).MustHaveHappened();
        }
    }
}
