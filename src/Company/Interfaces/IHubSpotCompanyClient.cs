using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.Company.Interfaces
{
    public interface IHubSpotCompanyClient
    {
        /// <summary>
        /// Creates the Company entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<T> CreateAsync<T>(ICompanyHubSpotEntity entity) where T : IHubSpotEntity, new();
        /// <summary>
        /// Delete a company from hubspot by id
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        Task DeleteAsync(long CompanyId);
        /// <summary>
        /// Get a Company by email address (only searches on domain)
        /// </summary>
        /// <param name="email"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetByEmailAsync<T>(string email, CompanySearchByDomain opts = null) where T : IHubSpotEntity, new();
        /// <summary>
        /// Return a single Company by id from hubspot
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetByIdAsync<T>(long CompanyId) where T : IHubSpotEntity, new();
        /// <summary>
        /// List Companies 
        /// </summary>
        /// <param name="opts">Request options - use for pagination</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> ListAsync<T>(CompanyListRequestOptions opts = null) where T : IHubSpotEntity, new();
        /// <summary>
        /// Update an existing company in hubspot
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> UpdateAsync<T>(ICompanyHubSpotEntity entity) where T : IHubSpotEntity, new();
        /// <summary>
        /// Resolve a hubspot API path based off the entity and operation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        string PathResolver(ICompanyHubSpotEntity entity, HubSpotAction action);
    }
}