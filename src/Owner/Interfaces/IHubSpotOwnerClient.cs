using Skarp.HubSpotClient.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.Owner.Interfaces
{
    public interface IHubSpotOwnerClient
    {
        /// <summary>
        /// Return a single Owner by id from hubspot
        /// </summary>
        /// <param name="ownerId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetByIdAsync<T>(long ownerId) where T : IOwnerHubSpotEntity, new();
        /// <summary>
        /// Return a single Owner by id from hubspot
        /// </summary>
        /// <param name="ownerId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetByIdAsync<T>(string ownerId) where T : IOwnerHubSpotEntity, new();
        /// <summary>
        /// List Owners 
        /// </summary>
        /// <param name="opts">Request options - use for filtering</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IEnumerable<T>> ListAsync<T>(OwnerListRequestOptions opts = null) where T : IOwnerHubSpotEntity, new();
        /// <summary>
        /// Resolve a hubspot API path based off the entity and operation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        string PathResolver(IOwnerHubSpotEntity entity, HubSpotAction action);
    }
}