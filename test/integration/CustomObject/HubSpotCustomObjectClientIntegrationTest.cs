using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using integration.CustomObject.Dto;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Contact;
using Skarp.HubSpotClient.Contact.Dto;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.CustomObjects;
using Xunit;
using Xunit.Abstractions;

namespace integration.CustomObject;

public class HubSpotCustomObjectClientIntegrationTest : IntegrationTestBase<HubSpotCustomObjectClient>
{
    private readonly HubSpotCustomObjectClient _client;
    private readonly string _apiKey;
    private readonly bool _isAppVeyorEnv;

    public HubSpotCustomObjectClientIntegrationTest(ITestOutputHelper output) : base(output)
    {
        _apiKey = Environment.GetEnvironmentVariable("HUBSPOT_API_KEY") ?? "demo";
        _isAppVeyorEnv = (Environment.GetEnvironmentVariable("APPVEYOR") ?? "false").Equals("true", StringComparison.InvariantCultureIgnoreCase);
        _client = new HubSpotCustomObjectClient(new RealRapidHttpClient(new HttpClient()),base.Logger,
                                                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                                                "https://api.hubapi.com",
                                                _apiKey);
    }

    [Fact]
    public async Task Create_custom_object_and_get_works()
    {
        if (_apiKey.Equals("demo") && _isAppVeyorEnv)
        {
            Output.WriteLine("Skipping test as the API key is incorrectly set and we're in AppVeyor");
            Assert.True(true);
            return;
        }

        var customObject = new CustomObjectHubSpotEntityExtended()
        {
            my_object_property = "Test 5"
        };
        var created = await _client.CreateAsync<CustomObjectHubSpotEntityExtended>(customObject);

        Assert.NotNull(created.my_object_property);

        var retrieved = await _client.GetByIdAsync<CustomObjectHubSpotEntityExtended>(created.Id.Value, new CustomObjectRequestOptions()
        {
            PropertiesToInclude = new List<string>()
            {
                "my_object_property"
            }
        });

        Assert.NotNull(retrieved);
        //Assert.Equal("2300", retrieved.ZipCode);
    }
}