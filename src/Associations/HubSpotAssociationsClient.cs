using Flurl;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Associations.Dto;
using Skarp.HubSpotClient.Associations.Interfaces;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Requests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.Associations
{
    public class HubSpotAssociationsClient : HubSpotBaseClient, IHubSpotAssociationsClient
    {
        /// <summary>
        /// Mockable and container ready constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        /// <param name="serializer"></param>
        /// <param name="hubSpotBaseUrl"></param>
        /// <param name="apiKey"></param>
        public HubSpotAssociationsClient(
            IRapidHttpClient httpClient,
            ILogger logger,
            RequestSerializer serializer,
            string hubSpotBaseUrl,
            string apiKey)
             : base(httpClient, logger, serializer, hubSpotBaseUrl, apiKey)
        {
        }

        /// <summary>
        /// Create a new instance of the HubSpotAssociationsClient with default dependencies
        /// </summary>
        /// <remarks>
        /// This constructor creates a HubSpotCompanyClient using "real" dependencies that will send requests 
        /// via the network - if you wish to have support for functional tests and mocking use the "eager" constructor
        /// that takes in all underlying dependecies
        /// </remarks>
        /// <param name="apiKey">Your API key</param>
        public HubSpotAssociationsClient(string apiKey)
        : base(
              new RealRapidHttpClient(new HttpClient()),
              NoopLoggerFactory.Get(),
              new RequestSerializer(new RequestDataConverter(NoopLoggerFactory.Get<RequestDataConverter>())),
              "https://api.hubapi.com",
              apiKey)
        { }

        /// <summary>
        /// Return a list of associations for a CRM object by id from hubspot based on the association definition id
        /// </summary>
        /// <param name="fromObjectId">Object ID for which to list its associations</param>
        /// <param name="definitionId">The definition ID of the associations to list</param>
        /// <param name="opts">Additional request options, use for limiting and pagination</param>
        /// <returns></returns>
        public async Task<IAssociationListHubSpotEntity<long>> GetListByIdAsync(long FromObjectId, HubSpotAssociationDefinitions definitionId, AssociationListRequestOptions opts = null)
        {
            Logger.LogDebug("Get associations for object with definition");
            if (opts == null)
            {
                opts = new AssociationListRequestOptions();
            }
            var path = PathResolver(new AssociationHubSpotEntity(), HubSpotAction.List)
                .Replace(":fromObjectId:", FromObjectId.ToString())
                .Replace(":definitionId:", ((int)definitionId).ToString())
                .SetQueryParam("limit", opts.NumberOfAssociationsToReturn);
            if (opts.AssociationOffset.HasValue)
            {
                path = path.SetQueryParam("offset", opts.AssociationOffset);
            }
            var data = await GetGenericAsync<AssociationListHubSpotEntity<long>>(path);
            return data;
        }

        /// <summary>
        /// Create association based on definition id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<bool> Create<T>(T entity)
        {
            Logger.LogDebug("Create association with definition");

            var path = PathResolver(new AssociationHubSpotEntity(), HubSpotAction.Create);
            var data = await PutOrPostGeneric(path, entity, false);
            return data;
        }

        /// <summary>
        /// Create in batch associations based on definition id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<bool> CreateBatch<T>(List<T> entities)
        {
            Logger.LogDebug("Create batch associations with definition");

            var path = PathResolver(new AssociationHubSpotEntity(), HubSpotAction.CreateBatch);
            var data = await PutOrPostBatch(path, entities, false);
            return data;
        }

        /// <summary>
        /// Delete association based on definition id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<bool> Delete<T>(T entity)
        {
            Logger.LogDebug("Create batch associations with definition");

            var path = PathResolver(new AssociationHubSpotEntity(), HubSpotAction.Delete);
            var data = await PutOrPostGeneric(path, entity, false);
            return data;
        }

        /// <summary>
        /// Delete in batch associations based on definition id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<bool> DeleteBatch<T>(List<T> entities)
        {
            Logger.LogDebug("Delete batch associations with definition");

            var path = PathResolver(new AssociationHubSpotEntity(), HubSpotAction.DeleteBatch);
            var data = await PutOrPostBatch(path, entities, false);
            return data;
        }

        /// <summary>
        /// Resolve a hubspot API path based off the entity and operation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string PathResolver(IAssociationHubSpotEntity entity, HubSpotAction action)
        {
            switch (action)
            {
                case HubSpotAction.Create:
                    return $"{entity.RouteBasePath}/associations";
                case HubSpotAction.CreateBatch:
                    return $"{entity.RouteBasePath}/associations/create-batch";
                case HubSpotAction.List:
                    return $"{entity.RouteBasePath}/associations/:fromObjectId:/HUBSPOT_DEFINED/:definitionId:";
                case HubSpotAction.Delete:
                    return $"{entity.RouteBasePath}/associations/delete";
                case HubSpotAction.DeleteBatch:
                    return $"{entity.RouteBasePath}/associations/delete-batch";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}
