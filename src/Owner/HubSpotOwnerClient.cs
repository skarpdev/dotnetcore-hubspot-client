using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Owner.Dto;
using Skarp.HubSpotClient.Owner.Interfaces;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.Core.Requests;
using System.Collections.Generic;
using System.Linq;

namespace Skarp.HubSpotClient.Owner
{
    public class HubSpotOwnerClient : HubSpotBaseClient
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
        /// Create a new instance of the HubSpotContactClient with default dependencies
        /// </summary>
        /// <remarks>
        /// This constructor creates a HubSpotContactClient using "real" dependencies that will send requests 
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
        /// Get a contact by email address
        /// </summary>
        /// <param name="email"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetByEmailAsync<T>(string email) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Contact get by email");
            var opt = new OwnerListRequestOptions() { EmailAddress = email };
            var owner = await ListAsync<OwnerListHubSpotEntity<OwnerHubSpotEntity>>(opt);
            var value = owner.Owners.FirstOrDefault();
            if (value != null)
                return (T)Convert.ChangeType(value, typeof(T));

            return default(T);
        }

        /// <summary>
        /// List contacts 
        /// </summary>
        /// <param name="opts">Request options - use for pagination</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> ListAsync<T>(OwnerListRequestOptions opts = null) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Owner ListAsync");
            if (opts == null)
            {
                opts = new OwnerListRequestOptions();
            }
            var path = PathResolver(new OwnerHubSpotEntity(), HubSpotAction.List);
            if (!string.IsNullOrEmpty(opts.EmailAddress))
            {
                path = path.SetQueryParam("email", opts.EmailAddress);
            }
            if (opts.IncludeInactive)
            {
                path = path.SetQueryParam("includeInactive", "true");
            }

            var data = await ListAsync<T>(path);
            return data;
        }

        /// <summary>
        /// Resolve a hubspot API path based off the entity and opreation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string PathResolver(IOwnerHubSpotEntity entity, HubSpotAction action)
        {
            switch (action)
            {
                //case HubSpotAction.Create:
                //    return $"{entity.RouteBasePath}/engagements";
                //case HubSpotAction.Get:
                //    return $"{entity.RouteBasePath}/contact/vid/:contactId:/profile";
                //case HubSpotAction.GetByEmail:
                //    return $"{entity.RouteBasePath}/contact/email/:contactEmail:/profile";
                case HubSpotAction.List:
                    return $"{entity.RouteBasePath}/owners";
                //case HubSpotAction.Update:
                //    return $"{entity.RouteBasePath}/contact/vid/:contactId:/profile";
                //case HubSpotAction.Delete:
                //    return $"{entity.RouteBasePath}/contact/vid/:contactId:";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}