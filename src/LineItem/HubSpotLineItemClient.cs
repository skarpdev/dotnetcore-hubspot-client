using Flurl;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.LineItem.Dto;
using Skarp.HubSpotClient.LineItem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.LineItem
{
    public class HubSpotLineItemClient : HubSpotBaseClient, IHubSpotLineItemClient
    {
        /// <summary>
        /// Mockable and container ready constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        /// <param name="serializer"></param>
        /// <param name="hubSpotBaseUrl"></param>
        /// <param name="apiKey"></param>
        public HubSpotLineItemClient(
            IRapidHttpClient httpClient,
            ILogger<HubSpotLineItemClient> logger,
            RequestSerializer serializer,
            string hubSpotBaseUrl,
            string apiKey)
            : base(httpClient, logger, serializer, hubSpotBaseUrl, apiKey)
        {
        }

        /// <summary>
        /// Create a new instance of the HubSpotLineItemClient with default dependencies
        /// </summary>
        /// <remarks>
        /// This constructor creates a HubSpotLineItemClient using "real" dependencies that will send requests 
        /// via the network - if you wish to have support for functional tests and mocking use the "eager" constructor
        /// that takes in all underlying dependecies
        /// </remarks>
        /// <param name="apiKey">Your API key</param>
        public HubSpotLineItemClient(string apiKey)
        : base(
              new RealRapidHttpClient(new HttpClient()),
              NoopLoggerFactory.Get(),
              new RequestSerializer(new RequestDataConverter(NoopLoggerFactory.Get<RequestDataConverter>())),
              "https://api.hubapi.com",
              apiKey)
        { }

        public Task<T> CreateAsync<T>(ILineItemHubSpotEntity entity) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Line Item CreateAsync");
            var path = PathResolver(entity, HubSpotAction.Create);

            var request = _serializer.SerializeEntityToNameValueList(entity);

            return SendRequestAsync(path, HttpMethod.Post, request, (response) =>
            {
                return _serializer.DeserializeEntity<T>(response);
            });
        }

        public Task<IEnumerable<T>> CreateBatchAsync<T>(IEnumerable<ILineItemHubSpotEntity> entities) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Line Item batch create");
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (!entities.Any()) throw new ArgumentException("Must provide Line Item entities to update");

            var path = PathResolver(new LineItemHubSpotEntity(), HubSpotAction.CreateBatch);

            var request = _serializer.SerializeEntitiesToNameValueList(entities.ToList());

            return SendRequestAsync(path, HttpMethod.Post, request, (response) =>
            {
                return _serializer.DeserializeEntities<T>(response);
            });
        }

        public Task DeleteAsync(long lineItemId)
        {
            if (lineItemId < 1)
            {
                throw new ArgumentException("lineItemId must be set!");
            }
            var path = PathResolver(new LineItemHubSpotEntity(), HubSpotAction.Delete)
                .Replace(":lineItemId:", lineItemId.ToString());

            return DeleteAsync<LineItemHubSpotEntity>(path);
        }

        public Task DeleteBatchAsync(ListOfLineItemIds lineItemIds)
        {
            if (lineItemIds == null) throw new ArgumentNullException(nameof(lineItemIds));
            if (!lineItemIds.Ids.Any()) throw new ArgumentException("Ids must have values");
            if (lineItemIds.Ids.Any(id => id < 1)) throw new ArgumentException("Values in Ids must be > 0");

            var path = PathResolver(new LineItemHubSpotEntity(), HubSpotAction.DeleteBatch);

            var request = _serializer.SerializeEntity(lineItemIds);
            return SendRequestAsync(path, HttpMethod.Post, request, _ => { return true; });
        }

        public Task<T> GetByIdAsync<T>(long lineItemId, LineItemGetRequestOptions opts = null) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Line Item Get by id");
            var path = PathResolver(new LineItemHubSpotEntity(), HubSpotAction.Get)
                .Replace(":lineItemId:", lineItemId.ToString());

            opts ??= new LineItemGetRequestOptions();

            if (opts.PropertiesToInclude.Any())
            {
                path = path.SetQueryParam("properties", opts.PropertiesToInclude);
            }
            if (opts.IncludeDeletes)
            {
                path = path.SetQueryParam("includeDeletes", "true");
            }

            return GetAsync<T>(path);
        }

        public Task<T> ListAsync<T>(LineItemListRequestOptions opts = null) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Line Item List");
            var path = PathResolver(new LineItemHubSpotEntity(), HubSpotAction.List);

            opts ??= new LineItemListRequestOptions();

            if (opts.PropertiesToInclude.Any())
            {
                path = path.SetQueryParam("properties", opts.PropertiesToInclude);
            }
            if (opts.Offset.HasValue)
            {
                path = path.SetQueryParam("offset", opts.Offset.Value);
            }

            return ListAsync<T>(path);
        }

        public Task<T> UpdateAsync<T>(ILineItemHubSpotEntity entity) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Line Item update w. id: {0}", entity.Id);
            if (entity.Id < 1)
            {
                throw new ArgumentException("Line Item entity must have an id set!");
            }
            var path = PathResolver(entity, HubSpotAction.Update)
                .Replace(":lineItemId:", entity.Id.ToString());

            var request = _serializer.SerializeEntityToNameValueList(entity);

            return SendRequestAsync(path, HttpMethod.Put, request, (response) =>
            {
                return _serializer.DeserializeEntity<T>(response);
            });
        }

        public Task<IEnumerable<T>> UpdateBatchAsync<T>(IEnumerable<ILineItemHubSpotEntity> entities) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Line Item batch update");
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (!entities.Any()) throw new ArgumentException("Must provide Line Item entities to update");

            var path = PathResolver(new LineItemHubSpotEntity(), HubSpotAction.UpdateBatch);

            var request = _serializer.SerializeEntities(entities.ToList());

            return SendRequestAsync(path, HttpMethod.Post, request, (response) =>
            {
                return _serializer.DeserializeEntities<T>(response);
            });
        }

        /// <summary>
        /// Resolve a hubspot API path based off the entity and operation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal string PathResolver(ILineItemHubSpotEntity entity, HubSpotAction action)
        {
            switch (action)
            {
                case HubSpotAction.Create:
                    return $"{entity.RouteBasePath}/line_items";
                case HubSpotAction.Get:
                    return $"{entity.RouteBasePath}/line_items/:lineItemId:";
                case HubSpotAction.Update:
                    return $"{entity.RouteBasePath}/line_items/:lineItemId:";
                case HubSpotAction.Delete:
                    return $"{entity.RouteBasePath}/line_items/:lineItemId:";
                case HubSpotAction.List:
                    return $"{entity.RouteBasePath}/line_items/paged";
                case HubSpotAction.CreateBatch:
                    return $"{entity.RouteBasePath}/line_items/batch-create";
                case HubSpotAction.DeleteBatch:
                    return $"{entity.RouteBasePath}/line_items/batch-delete";
                case HubSpotAction.UpdateBatch:
                    return $"{entity.RouteBasePath}/line_items/batch-update";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}
