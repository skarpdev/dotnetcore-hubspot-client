using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Company.Dto;
using Skarp.HubSpotClient.Company.Interfaces;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.Core.Requests;

namespace Skarp.HubSpotClient.Company
{
    public class HubSpotCompanyClient : HubSpotBaseClient
    {
        /// <summary>
        /// Mockable and container ready constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        /// <param name="serializer"></param>
        /// <param name="hubSpotBaseUrl"></param>
        /// <param name="apiKey"></param>
        public HubSpotCompanyClient(
            IRapidHttpClient httpClient,
            ILogger<HubSpotCompanyClient> logger,
            RequestSerializer serializer,
            string hubSpotBaseUrl,
            string apiKey)
            : base(httpClient, logger, serializer, hubSpotBaseUrl, apiKey)
        {
        }

        /// <summary>
        /// Create a new instance of the HubSpotCompanyClient with default dependencies
        /// </summary>
        /// <remarks>
        /// This constructor creates a HubSpotCompanyClient using "real" dependencies that will send requests 
        /// via the network - if you wish to have support for functional tests and mocking use the "eager" constructor
        /// that takes in all underlying dependecies
        /// </remarks>
        /// <param name="apiKey">Your API key</param>
        public HubSpotCompanyClient(string apiKey)
        : base(
              new RealRapidHttpClient(new HttpClient()), 
              NoopLoggerFactory.Get(), 
              new RequestSerializer(new RequestDataConverter(NoopLoggerFactory.Get<RequestDataConverter>())),
              "https://api.hubapi.com", 
              apiKey)
        { }

        /// <summary>
        /// Creates the Company entity asyncrhounously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> CreateAsync<T>(ICompanyHubSpotEntity entity) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Company CreateAsync");
            var path = PathResolver(entity, HubSpotAction.Create);
            var data = await PostAsync<T>(path, entity);
            return data;
        }

        /// <summary>
        /// Return a single Company by id from hubspot
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetByIdAsync<T>(long CompanyId) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Company Get by id ");
            var path = PathResolver(new CompanyHubSpotEntity(), HubSpotAction.Get)
                .Replace(":companyId:", CompanyId.ToString());
            var data = await GetAsync<T>(path);
            return data;
        }

        /// <summary>
        /// Get a Company by email address (only searches on domain)
        /// </summary>
        /// <param name="email"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetByEmailAsync<T>(string email, CompanySearchByDomain opts = null) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Company get by email domain");
            if (opts == null)
            {
                opts = new CompanySearchByDomain();
            }

            var path = PathResolver(new CompanyHubSpotEntity(), HubSpotAction.GetByEmail)
                .Replace(":domain:", email.Substring(email.IndexOf("@")+1));
            var data = await ListAsPostAsync<T>(path, opts);
            return data;
        }

        public async Task<T> UpdateAsync<T>(ICompanyHubSpotEntity entity) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Company update w. id: {0}", entity.Id);
            if (entity.Id < 1)
            {
                throw new ArgumentException("Company entity must have an id set!");
            }
            var path = PathResolver(entity, HubSpotAction.Update)
                .Replace(":companyId:", entity.Id.ToString());

            var data = await PutAsync<T>(path, entity);

            return data;
        }

        public async Task DeleteAsync(long CompanyId)
        {
            Logger.LogDebug("Company delete w. id: {0}", CompanyId);

            var path = PathResolver(new CompanyHubSpotEntity(), HubSpotAction.Delete)
                .Replace(":companyId:", CompanyId.ToString());

            await DeleteAsync<CompanyHubSpotEntity>(path);
        }

        /// <summary>
        /// Resolve a hubspot API path based off the entity and opreation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string PathResolver(ICompanyHubSpotEntity entity, HubSpotAction action)
        {
            switch (action)
            {
                case HubSpotAction.Create:
                    return $"{entity.RouteBasePath}/companies";
                case HubSpotAction.Get:
                    return $"{entity.RouteBasePath}/companies/:companyId:";
                case HubSpotAction.GetByEmail:
                    return $"{entity.RouteBasePath}/domains/:domain:/companies";
                case HubSpotAction.Update:
                    return $"{entity.RouteBasePath}/companies/:companyId:";
                case HubSpotAction.Delete:
                    return $"{entity.RouteBasePath}/companies/:companyId:";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}