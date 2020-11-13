using Skarp.HubSpotClient.Associations.Interfaces;
using Skarp.HubSpotClient.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skarp.HubSpotClient.Associations
{
    public interface IHubSpotAssociationsClient
    {
        /// <summary>
        /// Create association based on definition id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> Create<T>(T entity);
        /// <summary>
        /// Create in batch associations based on definition id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> CreateBatch<T>(List<T> entities);
        /// <summary>
        /// Delete association based on definition id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> Delete<T>(T entity);
        /// <summary>
        /// Delete in batch associations based on definition id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> DeleteBatch<T>(List<T> entities);
        /// <summary>
        /// Return a list of associations for a CRM object by id from hubspot based on the association definition id
        /// </summary>
        /// <param name="fromObjectId">Object ID for which to list its associations</param>
        /// <param name="definitionId">The definition ID of the associations to list</param>
        /// <param name="opts">Additional request options, use for limiting and pagination</param>
        /// <returns></returns>
        Task<IAssociationListHubSpotEntity<long>> GetListByIdAsync(long fromObjectId, HubSpotAssociationDefinitions definitionId, AssociationListRequestOptions opts = null);
    }
}