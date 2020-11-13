using Skarp.HubSpotClient.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.LineItem.Interfaces
{
    public interface IHubSpotLineItemClient
    {
        /// <summary>
        /// Creates the line item entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<T> CreateAsync<T>(ILineItemHubSpotEntity entity) where T : IHubSpotEntity, new();
        /// <summary>
        /// Creates as group of line items entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<IEnumerable<T>> CreateBatchAsync<T>(IEnumerable<ILineItemHubSpotEntity> entity) where T : IHubSpotEntity, new();
        /// <summary>
        /// Delete a line item from hubspot
        /// </summary>
        /// <param name="lineItemId"></param>
        /// <returns></returns>
        Task DeleteAsync(long lineItemId);
        /// <summary>
        /// Deletes multiple line items from hubspot
        /// </summary>
        /// <param name="lineItemIds"></param>
        /// <returns></returns>
        Task DeleteBatchAsync(ListOfLineItemIds lineItemIds);
        /// <summary>
        /// Return a single line item by id from hubspot
        /// </summary>
        /// <param name="lineItemId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetByIdAsync<T>(long lineItemId, LineItemGetRequestOptions opts = null) where T : IHubSpotEntity, new();
        /// <summary>
        /// Lists all line items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> ListAsync<T>(LineItemListRequestOptions opts = null) where T : IHubSpotEntity, new();
        /// <summary>
        /// Update an existing line item in hubspot
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> UpdateAsync<T>(ILineItemHubSpotEntity entity) where T : IHubSpotEntity, new();
        /// <summary>
        /// Update multiple existing line items in hubspot
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> UpdateBatchAsync<T>(IEnumerable<ILineItemHubSpotEntity> entities) where T : IHubSpotEntity, new();
    }
}
