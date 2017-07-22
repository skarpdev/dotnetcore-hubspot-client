using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Dto;
using Skarp.HubSpotClient.Interfaces;
using Skarp.HubSpotClient.Requests;
using Xunit;
using Xunit.Abstractions;

namespace Skarp.HubSpotClient.UnitTest
{
    public class HubSpotContactClientTest : UnitTestBase<HubSpotContactClient>
    {
        private readonly HubSpotContactClient _client;
        private IRapidHttpClient _mockHttpClient;
        private RequestSerializer _mockSerializer;

        public HubSpotContactClientTest(ITestOutputHelper output) : base(output)
        {
            _mockHttpClient = A.Fake<IRapidHttpClient>(opts => opts.Strict());

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored))
                .Returns(Task.FromResult(CreateNewEmptyOkResponse()));

            _mockSerializer = A.Fake<RequestSerializer>(opts => opts.Strict());
            A.CallTo(() => _mockSerializer.SerializeEntity(A<ContactHubSpotEntity>.Ignored))
                .Returns("{}");

            A.CallTo(() => _mockSerializer.DeserializeEntity<ContactHubSpotEntity>(A<string>.Ignored))
                .Returns(new ContactHubSpotEntity());

            _client = new HubSpotContactClient(
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
        public async Task ContactClient_create_contact_works()
        {
            var response = await _client.CreateAsync<ContactHubSpotEntity>(new ContactHubSpotEntity
            {
                FirstName = "Adrian",
                Lastname = "Baws",
                Email = "adrian@the-email.com"
            });

            Assert.NotNull(response);

            A.CallTo(() => _mockHttpClient.SendAsync(A<HttpRequestMessage>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.SerializeEntity(A<IHubSpotEntity>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockSerializer.DeserializeEntity<ContactHubSpotEntity>("{}")).MustHaveHappened();
        }
    }
}
