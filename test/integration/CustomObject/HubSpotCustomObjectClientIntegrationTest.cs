using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using integration.CustomObject.Dto;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
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
        _client = new HubSpotCustomObjectClient(new RealRapidHttpClient(new HttpClient()), Logger,
                                                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                                                "https://api.hubapi.com",
                                                _apiKey);
    }

    [Fact]
    public async Task Create_custom_object()
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

        Assert.True(created?.Id > 0);
    }

    [Fact]
    public async Task Create_custom_object_and_retrieve_it_using_id()
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

        Assert.True(created?.Id > 0);

        var retrieved = await _client.GetByIdAsync<CustomObjectHubSpotEntityExtended>(created.Id.Value);

        Assert.NotNull(retrieved);
    }

    [Fact]
    public async Task Create_custom_object_and_retrieve_with_custom_properties()
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

        Assert.True(created?.Id > 0);

        var retrieved = await _client.GetByIdAsync<CustomObjectHubSpotEntityExtended>(created.Id.Value, new CustomObjectRequestOptions()
        {
            PropertiesToInclude = new List<string>()
            {
                "my_object_property"
            }
        });

        Assert.True(retrieved.my_object_property == customObject.my_object_property);
    }

    [Fact]
    public async Task Create_custom_object_and_update_property()
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

        Assert.True(created?.Id > 0);
        var newValue = Guid.NewGuid().ToString();
        created.my_object_property = newValue;
        var updated = await _client.UpdateAsync<CustomObjectHubSpotEntityExtended>(created);
        Assert.True(updated.my_object_property == newValue);
    }

    [Fact]
    public async Task Create_custom_object_and_delete_it()
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

        Assert.True(created?.Id > 0);

        await _client.DeleteAsync(created);

        var deleted = await _client.GetByIdAsync<CustomObjectHubSpotEntityExtended>(created.Id.Value);

        Assert.Null(deleted);
    }
}