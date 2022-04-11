using System;
using System.Net.Http;
using System.Threading.Tasks;
using integration.UniqueIdentifiers.Dto;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.CustomObjects;
using Xunit;
using Xunit.Abstractions;

namespace integration.UniqueIdentifiers;

public class UniqueIdentifierClientIntegrationTest : IntegrationTestBase<HubSpotCustomObjectClient>
{
    private readonly HubSpotCustomObjectClient _client;
    private readonly string _apiKey;
    private readonly bool _isAppVeyorEnv;

    public UniqueIdentifierClientIntegrationTest(ITestOutputHelper output) : base(output)
    {
        _apiKey = Environment.GetEnvironmentVariable("HUBSPOT_API_KEY") ?? "demo";
        _isAppVeyorEnv = (Environment.GetEnvironmentVariable("APPVEYOR") ?? "false").Equals("true", StringComparison.InvariantCultureIgnoreCase);
        _client = new HubSpotCustomObjectClient(new RealRapidHttpClient(new HttpClient()), Logger,
                                                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>())),
                                                "https://api.hubapi.com",
                                                _apiKey);
    }

    [Fact]
    public async Task Gives_issue_when_trying_to_add_dups()
    {
        var rnd = new Random();
        if (_apiKey.Equals("demo") && _isAppVeyorEnv)
        {
            Output.WriteLine("Skipping test as the API key is incorrectly set and we're in AppVeyor");
            Assert.True(true);
            return;
        }

        var customObject = new UniqueIdentifierHubSpotEntityExtended()
        {
            my_primary_key = rnd.Next(int.MinValue, int.MaxValue),
            my_object_property = "Test"
        };
        var created = await _client.CreateAsync<UniqueIdentifierHubSpotEntityExtended>(customObject);

        Assert.True(created?.Id > 0);

        try
        {
            var _ = await _client.CreateAsync<UniqueIdentifierHubSpotEntityExtended>(customObject);
        }
        catch
        {
            Assert.True(true);
        }
    }

    [Fact]
    public async Task Get_by_id_using_custom_key()
    {
        var rnd = new Random();
        if (_apiKey.Equals("demo") && _isAppVeyorEnv)
        {
            Output.WriteLine("Skipping test as the API key is incorrectly set and we're in AppVeyor");
            Assert.True(true);
            return;
        }

        var customObject = new UniqueIdentifierHubSpotEntityExtended
        {
            my_primary_key = rnd.Next(int.MinValue, int.MaxValue),
            my_object_property = "Test"
        };
        var created = await _client.CreateAsync<UniqueIdentifierHubSpotEntityExtended>(customObject);

        var retrieved = await _client.GetByIdAsync<UniqueIdentifierHubSpotEntityExtended>(created.my_primary_key);

        Assert.True(retrieved?.Id > 0 && retrieved.my_primary_key == created.my_primary_key);
    }
}