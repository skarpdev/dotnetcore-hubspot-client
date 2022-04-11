using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.CustomObjects.Interfaces;
using System.Net.Http;
using System;
using System.Linq;
using Flurl;
using Skarp.HubSpotClient.Contact;
using Skarp.HubSpotClient.Contact.Dto;
using Skarp.HubSpotClient.CustomObjects.Dto;

namespace Skarp.HubSpotClient.CustomObjects;

public class HubSpotCustomObjectClient : HubSpotBaseClient , IHubSpotCustomObjectClient
{
    /// <summary>
    /// Mockable and container ready constructor
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="logger"></param>
    /// <param name="serializer"></param>
    /// <param name="hubSpotBaseUrl"></param>
    /// <param name="apiKey"></param>
    public HubSpotCustomObjectClient(
        IRapidHttpClient httpClient,
        ILogger<HubSpotCustomObjectClient> logger,
        RequestSerializer serializer,
        string hubSpotBaseUrl,
        string apiKey)
        : base(httpClient, logger, serializer, hubSpotBaseUrl, apiKey)
    {
    }

    /// <summary>
    /// Create a new instance of the HubSpotCustomObjectClient with default dependencies
    /// </summary>
    /// <remarks>
    /// This constructor creates a HubSpotCustomObjectClient using "real" dependencies that will send requests 
    /// via the network - if you wish to have support for functional tests and mocking use the "eager" constructor
    /// that takes in all underlying dependencies
    /// </remarks>
    /// <param name="apiKey">Your API key</param>
    public HubSpotCustomObjectClient(string apiKey)
        : base(
            new RealRapidHttpClient(new HttpClient()),
            NoopLoggerFactory.Get(),
            new RequestSerializer(new RequestDataConverter(NoopLoggerFactory.Get<RequestDataConverter>())),
            "https://api.hubapi.com",
            apiKey)
    { }

    public async Task<T> CreateAsync<T>(ICustomObjectHubSpotEntity entity) where T : ICustomObjectHubSpotEntity, IHubSpotEntity, new()
    {
        Logger.LogDebug("Custom Object CreateAsync");
        var path = PathResolver(entity, HubSpotAction.Create);
        var data = await PostAsync<T>(path, entity);
        return data;
    }

    public async Task<T> UpdateAsync<T>(ICustomObjectHubSpotEntity entity) where T : ICustomObjectHubSpotEntity, IHubSpotEntity, new()
    {
        Logger.LogDebug("Custom object update w. id: {0}", entity.Id);
        if (entity.Id < 1)
        {
            throw new ArgumentException("Custom object entity must have an id set!");
        }
        var path = PathResolver(entity, HubSpotAction.Update)
            .Replace(":customObjectId:", entity.Id.ToString());

        return await PatchAsync<T>(path, entity);
    }

    public async Task DeleteAsync(ICustomObjectHubSpotEntity entity)
    {
        Logger.LogDebug("Custom object delete w. id: {0}", entity.Id);

        var path = PathResolver(entity, HubSpotAction.Delete)
            .Replace(":customObjectId:", entity.Id.ToString());

        await DeleteAsync<CustomObjectHubSpotEntity>(path);
    }

    public async Task<T> GetByIdAsync<T>(long customObjectId, CustomObjectRequestOptions opts = null) where T : ICustomObjectHubSpotEntity, IHubSpotEntity, new()
    {
        Logger.LogDebug("Custom object get by id ");
        var path = PathResolver(new T(), HubSpotAction.Get)
            .Replace(":customObjectId:", customObjectId.ToString());

        path = AddGetRequestOptions(path, opts);
        var data = await GetAsync<T>(path);
        return data;
    }

    /// <summary>
    /// Resolve a hubspot API path based off the entity and operation that is about to happen
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public string PathResolver(ICustomObjectHubSpotEntity entity, HubSpotAction action)
    {
        if (string.IsNullOrEmpty(entity.ObjectTypeId))
            throw new ArgumentException($"Property {nameof(entity.ObjectTypeId)} must have a valid value");

        var suffix = $"?{GetCustomIdPropertyUrlAttribute(entity)}";

        return action switch
        {
            HubSpotAction.Create => $"{entity.RouteBasePath}/{entity.ObjectTypeId}",
            HubSpotAction.Get => $"{entity.RouteBasePath}/{entity.ObjectTypeId}/:customObjectId:{suffix}",
            HubSpotAction.List => $"{entity.RouteBasePath}/{entity.ObjectTypeId}",
            HubSpotAction.Update => $"{entity.RouteBasePath}/{entity.ObjectTypeId}/:customObjectId:",
            HubSpotAction.Delete => $"{entity.RouteBasePath}/{entity.ObjectTypeId}/:customObjectId:",
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
    }

    private static string AddGetRequestOptions(string path, CustomObjectRequestOptions opts = null)
    {
        var newPath = path;

        opts ??= new CustomObjectRequestOptions();
        if (opts.PropertiesToInclude.Any())
        {
            newPath = newPath.SetQueryParam("properties", opts.PropertiesToInclude);
        }

        return newPath;
    }
}