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
        Task<bool> Create<T>(T entity, HubSpotAssociationDefinitions definitionId);
        /// <summary>
        /// Create in batch associations based on definition id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> CreateBatch<T>(List<T> entities, HubSpotAssociationDefinitions definitionId);
        /// <summary>
        /// Delete association based on definition id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> Delete<T>(T entity, HubSpotAssociationDefinitions definitionId);
        /// <summary>
        /// Delete in batch associations based on definition id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> DeleteBatch<T>(List<T> entities, HubSpotAssociationDefinitions definitionId);
        /// <summary>
        /// Return a list of associations for a CRM object by id from hubspot based on the association definition id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetListByIdAsync<T>(long FromObjectId, HubSpotAssociationDefinitions definitionId, AssociationListRequestOptions opts = null);
    }
}