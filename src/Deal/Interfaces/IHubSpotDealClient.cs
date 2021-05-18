using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.Core.Interfaces;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.Deal.Interfaces
{
    public interface IHubSpotDealClient
    {
        /// <summary>
        /// Creates the deal entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<T> CreateAsync<T>(IDealHubSpotEntity entity) where T : IHubSpotEntity, new();
        /// <summary>
        /// Delete a deal from hubspot
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns></returns>
        Task DeleteAsync(long dealId);
        /// <summary>
        /// Return a single deal by id from hubspot
        /// </summary>
        /// <param name="dealId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetByIdAsync<T>(long dealId) where T : IHubSpotEntity, new();
        /// <summary>
        /// List Deals 
        /// </summary>
        /// <param name="opts">Request options - use for pagination</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> ListAsync<T>(DealListRequestOptions opts = null) where T : IHubSpotEntity, new();
        /// <summary>
        /// Update an existing deal in hubspot
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> UpdateAsync<T>(IDealHubSpotEntity entity) where T : IHubSpotEntity, new();
        /// <summary>
        /// List deal properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetPropertiesAsync<T>() where T : IHubSpotEntity, new();
    }
}