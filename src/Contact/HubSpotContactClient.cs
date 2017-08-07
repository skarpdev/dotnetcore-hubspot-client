using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Contact.Dto;
using Skarp.HubSpotClient.Contact.Interfaces;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.Core.Requests;

namespace Skarp.HubSpotClient.Contact
{
    public class HubSpotContactClient : HubSpotBaseClient
    {
        /// <summary>
        /// Mockable and container ready constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        /// <param name="serializer"></param>
        /// <param name="hubSpotBaseUrl"></param>
        /// <param name="apiKey"></param>
        public HubSpotContactClient(
            IRapidHttpClient httpClient,
            ILogger<HubSpotContactClient> logger,
            RequestSerializer serializer,
            string hubSpotBaseUrl,
            string apiKey)
            : base(httpClient, logger, serializer, hubSpotBaseUrl, apiKey)
        {
        }

        /// <summary>
        /// Create a new instance of the HubSpotContactClient with default dependencies
        /// </summary>
        /// <remarks>
        /// This constructor creates a HubSpotContactClient using "real" dependencies that will send requests 
        /// via the network - if you wish to have support for functional tests and mocking use the "eager" constructor
        /// that takes in all underlying dependecies
        /// </remarks>
        /// <param name="hubSpotBaseUrl"></param>
        /// <param name="apiKey"></param>
        public HubSpotContactClient(string hubSpotBaseUrl, string apiKey)
        : base(
              new RealRapidHttpClient(new HttpClient()), 
              NoopLoggerFactory.Get(), 
              new RequestSerializer(new RequestDataConverter(NoopLoggerFactory.Get<RequestDataConverter>())), 
              hubSpotBaseUrl, 
              apiKey)
        { }

        /// <summary>
        /// Creates the contact entity asyncrhounously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> CreateAsync<T>(IContactHubSpotEntity entity) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Contact CreateAsync");
            var path = PathResolver(entity, HubSpotAction.Create);
            var data = await PostAsync<T>(path, entity);
            return data;
        }

        /// <summary>
        /// Return a single contact by id from hubspot
        /// </summary>
        /// <param name="contactId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetByIdAsync<T>(long contactId) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Contact Get by id ");
            var path = PathResolver(new ContactHubSpotEntity(), HubSpotAction.Get)
                .Replace(":contactId:", contactId.ToString());
            var data = await GetAsync<T>(path);
            return data;
        }

        /// <summary>
        /// Get a contact by email address
        /// </summary>
        /// <param name="email"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetByEmailAsync<T>(string email) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Contact get by email");
            var path = PathResolver(new ContactHubSpotEntity(), HubSpotAction.GetByEmail)
                .Replace(":contactEmail:", email);
            var data = await GetAsync<T>(path);
            return data;
        }

        /// <summary>
        /// List contacts 
        /// </summary>
        /// <param name="opts">Request options - use for pagination</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> ListAsync<T>(ContactListRequestOptions opts = null) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Contact ListAsync");
            if (opts == null)
            {
                opts = new ContactListRequestOptions();
            }
            var path = PathResolver(new ContactListHubSpotEntity<ContactHubSpotEntity>(), HubSpotAction.List) +
                       $"&count={opts.NumberOfContactsToReturn}";
            if (opts.ContactOffset.HasValue)
            {
                path = path + $"?vidOffset={opts.ContactOffset}";
            }

            var data = await ListAsync<T>(path);
            return data;
        }

        public async Task<object> UpdateAsync(long contactId)
        {
            throw new NotImplementedException();
        }

        public async Task<object> DeleteAsync(long contactId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolve a hubspot API path based off the entity and opreation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string PathResolver(IHubSpotEntity entity, HubSpotAction action)
        {
            switch (action)
            {
                case HubSpotAction.Create:
                    return $"{entity.RouteBasePath}/contact";
                case HubSpotAction.Get:
                    return $"{entity.RouteBasePath}/contact/vid/:contactId:/profile";
                case HubSpotAction.GetByEmail:
                    return $"{entity.RouteBasePath}/contact/email/:contactEmail:/profile";
                case HubSpotAction.List:
                    return $"{entity.RouteBasePath}/lists/all/contacts/all";
                case HubSpotAction.Update:
                    return $"{entity.RouteBasePath}/contact/vid/:contactId:/profile";
                case HubSpotAction.Delete:
                    return $"{entity.RouteBasePath}/contact/vid/:contactId:";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}