using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.Core.Requests;

namespace Skarp.HubSpotClient.Core
{
    public abstract class HubSpotBaseClient
    {
        protected readonly IRapidHttpClient HttpClient;
        protected readonly ILogger Logger;
        protected readonly RequestSerializer _serializer;
        protected readonly string HubSpotBaseUrl;
        
        private readonly string _apiKey;

        protected HubSpotBaseClient(
            IRapidHttpClient httpClient,
            ILogger logger,
            RequestSerializer serializer,
            string hubSpotBaseUrl,
            string apiKey)
        {
            HttpClient = httpClient;
            Logger = logger;
            _serializer = serializer;
            HubSpotBaseUrl = hubSpotBaseUrl.TrimEnd('/');
            _apiKey = apiKey;
        }

        /// <summary>
        /// Send a post request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="absoluteUriPath"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task<T> PostAsync<T>(string absoluteUriPath, IHubSpotEntity entity)
            where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Post async for uri path: '{0}' with type: '{1}'", absoluteUriPath, entity.GetType().Name);
            var httpMethod = HttpMethod.Post;

            return await PutOrPost<T>(absoluteUriPath, entity, true);
        }

        protected async Task<T> PutAsync<T>(string absoluteUriPath, IHubSpotEntity entity)
            where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Post async for uri path: '{0}' with type: '{1}'", absoluteUriPath, entity.GetType().Name);
            var json = _serializer.SerializeEntity(entity);
            var httpMethod = HttpMethod.Post;

            return await PutOrPost<T>(absoluteUriPath, entity, false);
        }

        /// <summary>
        /// Internal method to allow support for PUT and POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="absoluteUriPath"></param>
        /// <param name="entity"></param>
        /// <param name="usePost"></param>
        /// <returns></returns>
        private async Task<T> PutOrPost<T>(string absoluteUriPath, IHubSpotEntity entity, bool usePost)
            where T : IHubSpotEntity, new()
        {
            var json = _serializer.SerializeEntity(entity);

            var data = await SendRequestAsync<T>(
                absoluteUriPath,
                usePost ? HttpMethod.Post : HttpMethod.Put,
                json,
                responseData => (T)_serializer.DeserializeEntity<T>(responseData));

            return data;
        }

        /// <summary>
        /// Internal method to allow support for PUT and POST of a batch list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="absoluteUriPath"></param>
        /// <param name="entity"></param>
        /// <param name="usePost"></param>
        /// <returns></returns>
        public async Task<bool> PutOrPostBatch<T>(string absoluteUriPath, List<T> entities, bool usePost)
        {
            var json = _serializer.SerializeEntities<T>(entities);

            var data = await SendRequestAsync<bool>(
                absoluteUriPath,
                usePost ? HttpMethod.Post : HttpMethod.Put,
                json,
                (_) => { return true; });

            // flipping the return value because we expect status code 204 (no content) on batch updates.
            // Because the response has no content, the default return value for bool is false, we want to report as true
            // if it attempted to deserialize without an error, that is also a problem so we flip the true returned from the
            // deserialize func above.
            return !data;
        }

        /// <summary>
        /// Internal method to allow support for PUT and POST of a generic entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="absoluteUriPath"></param>
        /// <param name="entity"></param>
        /// <param name="usePost"></param>
        /// <returns></returns>
        public async Task<bool> PutOrPostGeneric<T>(string absoluteUriPath, T entity, bool usePost)
        {
            var json = _serializer.SerializeEntity(entity);

            var data = await SendRequestAsync<bool>(
                absoluteUriPath,
                usePost ? HttpMethod.Post : HttpMethod.Put,
                json,
                (_) => { return true; });

            // flipping the return value because we expect status code 204 (no content) on updates.
            // Because the response has no content, the default return value for bool is false, we want to report as true
            // if it attempted to deserialize without an error, that is also a problem so we flip the true returned from the
            // deserialize func above.
            return !data;
        }

        /// <summary>
        /// Send a "list" request - GET but with special handling of the return data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="absoluteUriPath"></param>
        /// <returns></returns>
        protected async Task<T> ListAsync<T>(string absoluteUriPath) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("List async for uri path: '{0}'", absoluteUriPath);
            var httpMethod = HttpMethod.Get;

            var data = await SendRequestAsync<T>(
                absoluteUriPath,
                httpMethod,
                null,
                responseData => (T) _serializer.DeserializeListEntity<T>(responseData));

            return data;
        }

        /// <summary>
        /// Send a "list" request / search request but using POST so that a body can be sent along
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="absoluteUriPath"></param>
        /// <returns></returns>
        protected async Task<T> ListAsPostAsync<T>(string absoluteUriPath, object entity) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("List async for uri path: '{0}'", absoluteUriPath);
            var json = _serializer.SerializeEntity(entity);

            var httpMethod = HttpMethod.Post;

            var data = await SendRequestAsync<T>(
                absoluteUriPath,
                httpMethod,
                json,
                responseData => (T)_serializer.DeserializeListEntity<T>(responseData));

            return data;
        }

        /// <summary>
        /// Send a get request
        /// </summary>
        /// <param name="absoluteUriPath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected async Task<T> GetAsync<T>(string absoluteUriPath) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Get async for uri path: '{0}'", absoluteUriPath);
            var httpMethod = HttpMethod.Get;

            var data = await SendRequestAsync<T>(
                absoluteUriPath,
                httpMethod,
                null,
                responseData => (T) _serializer.DeserializeEntity<T>(responseData)
            );

            return data;
        }

        /// <summary>
        /// Send a get request
        /// </summary>
        /// <param name="absoluteUriPath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected async Task<T> GetGenericAsync<T>(string absoluteUriPath)
        {
            Logger.LogDebug("Get async for uri path: '{0}'", absoluteUriPath);
            var httpMethod = HttpMethod.Get;

            var data = await SendRequestAsync<T>(
                absoluteUriPath,
                httpMethod,
                null,
                responseData => (T)_serializer.DeserializeGenericEntity<T>(responseData)
            );

            return data;
        }

        protected async Task DeleteAsync<T>(string absoluteUriPath) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Delete async for uri path: '{0}'", absoluteUriPath);
            var httpMethod = HttpMethod.Delete;

            await SendRequestAsync<T>(
                absoluteUriPath,
                httpMethod,
                null,
                responseData => (T) _serializer.DeserializeEntity<T>(responseData));
        }
        /// <summary>
        /// Helper method for dispatching the request and dealing with response errors
        /// </summary>
        /// <typeparam name="T">The type to deserialize as</typeparam>
        /// <param name="absoluteUriPath">The absolute path and query params to include in the request</param>
        /// <param name="httpMethod">HTTP method to use for the request</param>
        /// <param name="json">Optional json to send with the request</param>
        /// <param name="deserializeFunc">Func to handle deserialization of data when the request goes well</param>
        /// <returns>A deserialized entity with data when things go well, exceptionns otherwise</returns>
        protected async Task<T> SendRequestAsync<T>(string absoluteUriPath, HttpMethod httpMethod, string json,
            Func<string, T> deserializeFunc)
        {
            var fullUrl = $"{HubSpotBaseUrl}{absoluteUriPath}"
                .SetQueryParam("hapikey", _apiKey);
            
            Logger.LogDebug("Full url: '{0}'", fullUrl);

            var request = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(fullUrl)
            };
            if (!string.IsNullOrWhiteSpace(json))
            {
                request.Content = new JsonContent(json);
            }

            var response = await HttpClient.SendAsync(request);
            var responseData = "";
            if (response.Content != null)
            {
                responseData = await response.Content.ReadAsStringAsync();
            }

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return default(T);
                }

                throw new HubSpotException("Error from HubSpot", responseData, response);
            }
            if (string.IsNullOrWhiteSpace(responseData))
            {
                return default(T);
            }

            return deserializeFunc(responseData);
        }
    }
}