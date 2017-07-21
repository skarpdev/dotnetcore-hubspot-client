using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using RapidCore.Threading;
using Skarp.HubSpotClient.Interfaces;
using Skarp.HubSpotClient.Requests;

namespace Skarp.HubSpotClient
{
    public class HubSpotContactClient : HubSpotBaseClient
    {

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

        public async Task<object> GetAsync(long contactId)
        {
            throw new NotImplementedException();
        }

        public async Task<object> ListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<object> UpdateAsync(long contactId)
        {
            throw new NotImplementedException();
        }

        public async Task<object> DeleteAsync(long contactId)
        {
            throw new NotImplementedException();
        }

        public string PathResolver(IContactHubSpotEntity entity, HubSpotAction action)
        {
            switch (action)
            {
                case HubSpotAction.Create:
                    return $"{entity.RouteBasePath}/contact";
                case HubSpotAction.Get:
                    return $"{entity.RouteBasePath}/contact/vid/:contactId:/profile";
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
