using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.Deal.Dto;
using Skarp.HubSpotClient.Deal.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.Deal
{
    public class HubSpotDealClient : HubSpotBaseClient
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
        /// Creates the deal entity asyncrhounously.
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

        /// <summary>
        /// Resolve a hubspot API path based off the entity and opreation that is about to happen
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
                case HubSpotAction.Update:
                    return $"{entity.RouteBasePath}/deal/:dealId:";
                case HubSpotAction.Delete:
                    return $"{entity.RouteBasePath}/deal/:dealId:";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}
