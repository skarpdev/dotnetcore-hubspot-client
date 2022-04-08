using System;
using System.Threading.Tasks;
using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.CustomObjects.Interfaces;

public interface IHubSpotCustomObjectClient
{
    /// <summary>
    /// Creates a custom object entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task<T> CreateAsync<T>(ICustomObjectHubSpotEntity entity) where T : ICustomObjectHubSpotEntity, IHubSpotEntity, new();
    /// <summary>
    /// Update an existing custom object in hubspot
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<T> UpdateAsync<T>(ICustomObjectHubSpotEntity entity) where T : ICustomObjectHubSpotEntity, IHubSpotEntity, new();
    /// <summary>
    /// Delete an existing custom object in hubspot by id
    /// </summary>
    /// <param name="customObjectId"></param>
    /// <returns></returns>
    Task DeleteAsync(long customObjectId);
    /// <summary>
    /// Return a single custom object by id from hubspot
    /// </summary>
    /// <param name="customObjectId"></param>
    /// <param name="opts">Options for enabling/disabling history and specifying properties</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> GetByIdAsync<T>(long customObjectId, CustomObjectRequestOptions opts = null) where T : ICustomObjectHubSpotEntity, IHubSpotEntity, new();
    /// <summary>
    /// Lists custom objects
    /// </summary>
    /// <param name="opts">Request options - use for pagination</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> ListAsync<T>(CustomObjectRequestOptions opts = null) where T : ICustomObjectHubSpotEntity, IHubSpotEntity, new();
    
}
