using Skarp.HubSpotClient.Contact.Interfaces;
using Skarp.HubSpotClient.Core;
using Skarp.HubSpotClient.ListOfContacts.Dto;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.ListOfContacts.Interfaces
{
    public interface IHubSpotListOfContactsClient
    {
        /// <summary>
        /// Return a list of contacts for a contact list by id from hubspot
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetListByIdAsync<T>(long listId, ListOfContactsRequestOptions opts = null);
        /// <summary>
        /// Add list of contacts based on list id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> AddBatchAsync(HubSpotListOfContactsEntity contacts, long listId);
        /// <summary>
        /// Remove list of contacts based on list id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> RemoveBatchAsync(HubSpotListOfContactsEntity contacts, long listId);
        /// <summary>
        /// Resolve a hubspot API path based off the entity and operation that is about to happen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        string PathResolver(IContactHubSpotEntity entity, HubSpotAction action);
    }
}