using Flurl;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.Owner.Dto;
using Skarp.HubSpotClient.Owner.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.Owner
{
    public class HubSpotOwnerClient : HubSpotBaseClient, IHubSpotOwnerClient
    {
        /// <summary>
        /// Mockable and container ready constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        /// <param name="serializer"></param>
        /// <param name="hubSpotBaseUrl"></param>
        /// <param name="apiKey"></param>
        public HubSpotOwnerClient(
            IRapidHttpClient httpClient,
            ILogger<HubSpotOwnerClient> logger,
            RequestSerializer serializer,
            string hubSpotBaseUrl,
            string apiKey)
            : base(httpClient, logger, serializer, hubSpotBaseUrl, apiKey)
        {
        }

        /// <summary>
        /// Create a new instance of the HubSpotOwnerClient with default dependencies
        /// </summary>
        /// <remarks>
        /// This constructor creates a HubSpotOwnerClient using "real" dependencies that will send requests 
        /// via the network - if you wish to have support for functional tests and mocking use the "eager" constructor
        /// that takes in all underlying dependecies
        /// </remarks>
        /// <param name="apiKey">Your API key</param>
        public HubSpotOwnerClient(string apiKey)
        : base(
              new RealRapidHttpClient(new HttpClient()),
              NoopLoggerFactory.Get(),
              new RequestSerializer(new RequestDataConverter(NoopLoggerFactory.Get<RequestDataConverter>())),
              "https://api.hubapi.com",
              apiKey)
        { }

        /// <summary>
        /// Return a single Owner by id from hubspot
        /// </summary>
        /// <param name="ownerId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<T> GetByIdAsync<T>(long ownerId) where T : IOwnerHubSpotEntity, new() => GetByIdAsync<T>(ownerId.ToString());

        /// <summary>
        /// Return a single Owner by id from hubspot
        /// </summary>
        /// <param name="ownerId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetByIdAsync<T>(string ownerId) where T : IOwnerHubSpotEntity, new()
        {
            Logger.LogDebug("Owner Get by id");
            var path = PathResolver(new OwnerHubSpotEntity(), HubSpotAction.Get)
                .Replace(":ownerId:", ownerId);
            var data = await GetGenericAsync<T>(path);
            return data;
        }

        /// <summary>
        /// List Owners 
        /// </summary>
        /// <param name="opts">Request options - use for filtering</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListAsync<T>(OwnerListRequestOptions opts = null) where T : IOwnerHubSpotEntity, new()
        {
            Logger.LogDebug("Owner ListAsync");
            if (opts == null)
            {
                opts = new OwnerListRequestOptions();
            }
            var path = PathResolver(new OwnerHubSpotEntity(), HubSpotAction.List);
            if (opts.IncludeInactive)
            {
                path = path.SetQueryParam("includeinactive", "true");
            }
            if (!string.IsNullOrWhiteSpace(opts.Email))
            {
                path = path.SetQueryParam("email", opts.Email);
            }

            var data = await GetGenericAsync<IEnumerable<T>>(path);
            return data;
        }

        /// <summary>
        /// Resolve a hubspot API path based off the entity and operation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string PathResolver(IOwnerHubSpotEntity entity, HubSpotAction action)
        {
            switch (action)
            {
                case HubSpotAction.List:
                    return $"{entity.RouteBasePath}/owners";
                case HubSpotAction.Get:
                    return $"{entity.RouteBasePath}/owners/:ownerId:";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}
