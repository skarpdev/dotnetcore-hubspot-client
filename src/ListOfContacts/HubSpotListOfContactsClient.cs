using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Contact.Dto;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.ListOfContacts.Dto;
using Skarp.HubSpotClient.ListOfContacts.Interfaces;

namespace Skarp.HubSpotClient.ListOfContacts
{
    public class HubSpotListOfContactsClient : HubSpotBaseClient, IHubSpotListOfContactsClient
    {
        /// <summary>
        /// Mockable and container ready constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        /// <param name="serializer"></param>
        /// <param name="hubSpotBaseUrl"></param>
        /// <param name="apiKey"></param>
        public HubSpotListOfContactsClient(
            IRapidHttpClient httpClient,
            ILogger<HubSpotListOfContactsClient> logger,
            RequestSerializer serializer,
            string hubSpotBaseUrl,
            string apiKey)
            : base(httpClient, logger, serializer, hubSpotBaseUrl, apiKey)
        {
        }

        /// <summary>
        /// Create a new instance of the HubSpotListOfContactsClient with default dependencies
        /// </summary>
        /// <remarks>
        /// This constructor creates a HubSpotListOfContactsClient using "real" dependencies that will send requests 
        /// via the network - if you wish to have support for functional tests and mocking use the "eager" constructor
        /// that takes in all underlying dependecies
        /// </remarks>
        /// <param name="apiKey">Your API key</param>
        public HubSpotListOfContactsClient(string apiKey)
        : base(
              new RealRapidHttpClient(new HttpClient()),
              NoopLoggerFactory.Get(),
              new RequestSerializer(new RequestDataConverter(NoopLoggerFactory.Get<RequestDataConverter>())),
              "https://api.hubapi.com",
              apiKey)
        { }


        /// <summary>
        /// Creates the a new contact list entity asynchronously.
        /// https://legacydocs.hubspot.com/docs/methods/lists/create_list
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> CreateAsync<T>(CreateContactListRequestHubSpotEntity payload) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("ContactList CreateAsync");
            var path = PathResolver(new ContactHubSpotEntity(), HubSpotAction.Lists);
            var data = await PutOrPostGeneric<T>(path, payload, true, false);
            return data;
        }

        private async Task<T> PutOrPostGeneric<T>(string absoluteUriPath, ICreateContactListRequestHubSpotEntity entity, bool usePost, bool convertEntity) where T : IHubSpotEntity, new()
        {
            string json = null;
            try
            {
                json = _serializer.SerializeEntity(entity, convertEntity);
            }
            catch (Exception ex)
            {
                Logger.LogDebug("PutOrPostGeneric failed with exception: " + ex.ToString());
                throw;
            }

            return await SendRequestAsync<T>(
                absoluteUriPath,
                usePost ? HttpMethod.Post : HttpMethod.Put,
                json,
                responseData => (T)_serializer.DeserializeGenericEntity<T>(responseData));

        }


        /// <summary>
        /// Delete a list of contact by contact list id from hubspot
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task DeleteAsync(long listId)
        {
            if (listId < 1)
            {
                throw new ArgumentException("listId must be set!");
            }
            var path = PathResolver(new ContactHubSpotEntity(), HubSpotAction.Delete)
                .Replace(":listId:", listId.ToString());

            return DeleteAsync<ContactHubSpotEntity>(path); 
        }


        /// <summary>
        /// Return a list of contacts for a contact list by id from hubspot
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetListByIdAsync<T>(long listId, ListOfContactsRequestOptions opts = null)
        {
            Logger.LogDebug("Get contacts for list with id");
            if (opts == null)
            {
                opts = new ListOfContactsRequestOptions();
            }
            var path = PathResolver(new ContactHubSpotEntity(), HubSpotAction.Get)
                .Replace(":listId:", listId.ToString())
                .SetQueryParam("count", opts.NumberOfContactsToReturn);
            if (opts.ContactOffset.HasValue)
            {
                path = path.SetQueryParam("vidOffset", opts.ContactOffset);
            }
            var data = await GetGenericAsync<T>(path);
            return data;
        }

        /// <summary>
        /// Return a list of contact lists from hubspot
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetListAsync<T>(ListOfContactListsRequestOptions opts = null)
        {
            Logger.LogDebug("Get list of contact lists");
            if (opts == null)
            {
                opts = new ListOfContactListsRequestOptions();
            }
            var path = PathResolver(new ContactHubSpotEntity(), HubSpotAction.Lists)
                .SetQueryParam("count", opts.NumberOfContactListsToReturn);
            if (opts.ContactListOffset.HasValue)
            {
                path = path.SetQueryParam("vidOffset", opts.ContactListOffset);
            }
            var data = await GetGenericAsync<T>(path);
            return data;
        }

        /// <summary>
        /// Add list of contacts based on list id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<bool> AddBatchAsync(HubSpotListOfContactsEntity contacts, long listId)
        {
            Logger.LogDebug("Add batch of contacts to list of contacts with specified id");

            var path = PathResolver(new ContactHubSpotEntity(), HubSpotAction.CreateBatch)
                .Replace(":listId:", listId.ToString());
            var data = await PutOrPostGeneric(path, contacts, true);
            return data;
        }

        /// <summary>
        /// Remove list of contacts based on list id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<bool> RemoveBatchAsync(HubSpotListOfContactsEntity contacts, long listId)
        {
            Logger.LogDebug("Remove batch of contacts to list of contacts with specified id");

            var path = PathResolver(new ContactHubSpotEntity(), HubSpotAction.DeleteBatch)
                .Replace(":listId:", listId.ToString());
            var data = await PutOrPostGeneric(path, contacts, true);
            return data;
        }

        /// <summary>
        /// Resolve a hubspot API path based off the entity and operation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string PathResolver(Contact.Interfaces.IContactHubSpotEntity entity, HubSpotAction action)
        {
            switch (action)
            {
                case HubSpotAction.Get:
                    return $"{entity.RouteBasePath}/lists/:listId:/contacts/all";
                case HubSpotAction.Lists:
                    return $"{entity.RouteBasePath}/lists";
                case HubSpotAction.CreateBatch:
                    return $"{entity.RouteBasePath}/lists/:listId:/add";
                case HubSpotAction.Delete:
                    return $"{entity.RouteBasePath}/lists/:listId:";
                case HubSpotAction.DeleteBatch:
                    return $"{entity.RouteBasePath}/lists/:listId:/remove";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}
