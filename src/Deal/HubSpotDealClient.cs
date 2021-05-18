using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.Deal.Dto;
using Skarp.HubSpotClient.Deal.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Skarp.HubSpotClient.Common.Interfaces;

namespace Skarp.HubSpotClient.Deal
{
    public class HubSpotDealClient : HubSpotBaseClient, IHubSpotDealClient
    {
        /// <summary>
        /// Mockable and container ready constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        /// <param name="serializer"></param>
        /// <param name="hubSpotBaseUrl"></param>
        /// <param name="apiKey"></param>
        public HubSpotDealClient(
            IRapidHttpClient httpClient,
            ILogger<HubSpotDealClient> logger,
            RequestSerializer serializer,
            string hubSpotBaseUrl,
            string apiKey)
            : base(httpClient, logger, serializer, hubSpotBaseUrl, apiKey)
        {
        }

        /// <summary>
        /// Create a new instance of the HubSpotDealClient with default dependencies
        /// </summary>
        /// <remarks>
        /// This constructor creates a HubSpotDealClient using "real" dependencies that will send requests 
        /// via the network - if you wish to have support for functional tests and mocking use the "eager" constructor
        /// that takes in all underlying dependecies
        /// </remarks>
        /// <param name="apiKey">Your API key</param>
        public HubSpotDealClient(string apiKey)
        : base(
              new RealRapidHttpClient(new HttpClient()),
              NoopLoggerFactory.Get(),
              new RequestSerializer(new RequestDataConverter(NoopLoggerFactory.Get<RequestDataConverter>())),
              "https://api.hubapi.com",
              apiKey)
        { }

        /// <summary>
        /// Creates the deal entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> CreateAsync<T>(IDealHubSpotEntity entity) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Deal CreateAsync");
            var path = PathResolver(entity, HubSpotAction.Create);
            var data = await PostAsync<T>(path, entity);
            return data;
        }

        /// <summary>
        /// Return a single deal by id from hubspot
        /// </summary>
        /// <param name="dealId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetByIdAsync<T>(long dealId) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Deal Get by id ");
            var path = PathResolver(new DealHubSpotEntity(), HubSpotAction.Get)
                .Replace(":dealId:", dealId.ToString());
            var data = await GetAsync<T>(path);
            return data;
        }

        /// <summary>
        /// List Deals 
        /// </summary>
        /// <param name="opts">Request options - use for pagination</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> ListAsync<T>(DealListRequestOptions opts = null) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Deal ListAsync");
            if (opts == null)
            {
                opts = new DealListRequestOptions();
            }
            var path = PathResolver(new DealHubSpotEntity(), HubSpotAction.List)
                .SetQueryParam("limit", opts.NumberOfDealsToReturn);
            if (opts.DealOffset.HasValue)
            {
                path = path.SetQueryParam("offset", opts.DealOffset);
            }
            if (opts.PropertiesToInclude.Any())
                path.SetQueryParam("properties", opts.PropertiesToInclude);

            var data = await ListAsync<T>(path);
            return data;
        }

        public async Task<T> UpdateAsync<T>(IDealHubSpotEntity entity) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Deal update w. id: {0}", entity.Id);
            if (entity.Id < 1)
            {
                throw new ArgumentException("Deal entity must have an id set!");
            }
            var path = PathResolver(entity, HubSpotAction.Update)
                .Replace(":dealId:", entity.Id.ToString());

            var data = await PutAsync<T>(path, entity);
            return data;
        }

        public async Task DeleteAsync(long dealId)
        {
            Logger.LogDebug("Deal delete w. id: {0}", dealId);

            var path = PathResolver(new DealHubSpotEntity(), HubSpotAction.Delete)
                .Replace(":dealId:", dealId.ToString());

            await DeleteAsync<DealHubSpotEntity>(path);
        }

        public async Task<T> GetPropertiesAsync<T>() where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Deal GetPropertiesAsync");

            var path = PathResolver(new DealPropertyHubSpotEntity(), HubSpotAction.List);

            var data = await ListPropertiesAsync<T>(path);

            return data;
        }

        /// <summary>
        /// Resolve a hubspot API path based off the entity and operation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string PathResolver(IDealHubSpotEntity entity, HubSpotAction action)
        {
            switch (action)
            {
                case HubSpotAction.Create:
                    return $"{entity.RouteBasePath}/deal";
                case HubSpotAction.Get:
                    return $"{entity.RouteBasePath}/deal/:dealId:";
                case HubSpotAction.List:
                    return $"{entity.RouteBasePath}/deal/paged";
                case HubSpotAction.Update:
                    return $"{entity.RouteBasePath}/deal/:dealId:";
                case HubSpotAction.Delete:
                    return $"{entity.RouteBasePath}/deal/:dealId:";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        /// <summary>
        /// Resolve a hubspot API path based off the entity and operation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string PathResolver(IDealPropertyHubSpotEntity entity, HubSpotAction action)
        {
            switch (action)
            {
                case HubSpotAction.List:
                    return $"{entity.RouteBasePath}/deals/properties";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}
