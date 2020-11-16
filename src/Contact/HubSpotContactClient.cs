using System;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Contact.Dto;
using Skarp.HubSpotClient.Contact.Interfaces;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.Core.Requests;

namespace Skarp.HubSpotClient.Contact
{
    public class HubSpotContactClient : HubSpotBaseClient, IHubSpotContactClient
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
        /// <param name="apiKey">Your API key</param>
        public HubSpotContactClient(string apiKey)
        : base(
              new RealRapidHttpClient(new HttpClient()),
              NoopLoggerFactory.Get(),
              new RequestSerializer(new RequestDataConverter(NoopLoggerFactory.Get<RequestDataConverter>())),
              "https://api.hubapi.com",
              apiKey)
        { }

        /// <summary>
        /// Creates the contact entity asynchronously.
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
        /// Creates or updates the contact entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> CreateOrUpdateAsync<T>(IContactHubSpotEntity entity) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Contact CreateOrUpdateAsync");
            var path = PathResolver(entity, HubSpotAction.CreateOrUpdate)
                .Replace(":contactemail:", WebUtility.UrlEncode(entity.Email));
            var data = await PostAsync<T>(path, entity);
            return data;
        }

        /// <summary>
        /// Return a single contact by id from hubspot
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="opts">Options for enabling/disabling history and specifying properties</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetByIdAsync<T>(long contactId, ContactGetRequestOptions opts = null) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Contact Get by id ");
            var path = PathResolver(new ContactHubSpotEntity(), HubSpotAction.Get)
                .Replace(":contactId:", contactId.ToString());

            path = AddGetRequestOptions(path, opts);

            var data = await GetAsync<T>(path);
            return data;
        }

        /// <summary>
        /// Get a contact by email address
        /// </summary>
        /// <param name="email"></param>
        /// <param name="opts">Options for enabling/disabling history and specifying properties</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetByEmailAsync<T>(string email, ContactGetRequestOptions opts = null) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Contact get by email");
            var path = PathResolver(new ContactHubSpotEntity(), HubSpotAction.GetByEmail)
                .Replace(":contactEmail:", email);

            path = AddGetRequestOptions(path, opts);

            var data = await GetAsync<T>(path);
            return data;
        }

        private string AddGetRequestOptions(string path, ContactGetRequestOptions opts = null)
        {
            var newPath = path;

            opts ??= new ContactGetRequestOptions();
            if (opts.PropertiesToInclude.Any())
            {
                newPath = newPath.SetQueryParam("property", opts.PropertiesToInclude);
            }
            if (!opts.IncludeHistory)
            {
                newPath = newPath.SetQueryParam("propertyMode", "value_only");
            }
            newPath = newPath.SetQueryParam("formSubmissionMode", opts.FormSubmissionMode.ToString().ToLowerInvariant());
            newPath = newPath.SetQueryParam("showListMemberships", opts.IncludeListMemberships.ToString().ToLowerInvariant());

            return newPath;
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
            var path = PathResolver(new ContactHubSpotEntity(), HubSpotAction.List)
                .SetQueryParam("count", opts.NumberOfContactsToReturn);
            if (opts.ContactOffset.HasValue)
            {
                path = path.SetQueryParam("vidOffset", opts.ContactOffset);
            }

            if (opts.PropertiesToInclude.Any())
            {
                opts.PropertiesToInclude.ForEach(prop =>
                {
                    path.QueryParams.Add("property", prop);
                });
            }

            var data = await ListAsync<T>(path);
            return data;
        }

        public async Task UpdateAsync(IContactHubSpotEntity contact)
        {
            Logger.LogDebug("Contact update w. id: {0}", contact.Id);
            if (contact.Id < 1)
            {
                throw new ArgumentException("Contact entity must have an id set!");
            }
            var path = PathResolver(contact, HubSpotAction.Update)
                .Replace(":contactId:", contact.Id.ToString());

            await PostAsync<ContactHubSpotEntity>(path, contact);
        }

        public async Task DeleteAsync(long contactId)
        {
            Logger.LogDebug("Contact delete w. id: {0}", contactId);

            var path = PathResolver(new ContactHubSpotEntity(), HubSpotAction.Delete)
                .Replace(":contactId:", contactId.ToString());

            await DeleteAsync<ContactHubSpotEntity>(path);
        }

        /// <summary>
        /// Resolve a hubspot API path based off the entity and operation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string PathResolver(IContactHubSpotEntity entity, HubSpotAction action)
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
                case HubSpotAction.CreateOrUpdate:
                    return $"{entity.RouteBasePath}/contact/createOrUpdate/email/:contactemail:";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}
