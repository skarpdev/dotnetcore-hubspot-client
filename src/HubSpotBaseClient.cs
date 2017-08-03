using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Network;
using Skarp.HubSpotClient.Interfaces;
using Skarp.HubSpotClient.Requests;

namespace Skarp.HubSpotClient
{
    public abstract class HubSpotBaseClient
    {
        protected readonly IRapidHttpClient HttpClient;
        protected readonly ILogger Logger;
        private readonly RequestSerializer _serializer;
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
            HubSpotBaseUrl = hubSpotBaseUrl;
            _apiKey = apiKey;
        }

        /// <summary>
        /// Send a post request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="absoluteUriPath"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task<T> PostAsync<T>(string absoluteUriPath, IHubSpotEntity entity) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Post async for uri path: '{0}' with type: '{1}'", absoluteUriPath, entity.GetType().Name);
            var json = _serializer.SerializeEntity(entity);
            var httpMethod = HttpMethod.Post;

            var data = await SendRequestAsync<T>(
                absoluteUriPath,
                httpMethod,
                json,
                responseData => (T)_serializer.DeserializeEntity<T>(responseData));

            return data;
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
                responseData => (T)_serializer.DeserializeEntity<T>(responseData)
            );

            return data;
        }
        /// <summary>
        /// Helper method for dispatching the requet and dealing with response errors
        /// </summary>
        /// <typeparam name="T">The type to deserialize as</typeparam>
        /// <param name="absoluteUriPath">The absolute path and query params to include in the request</param>
        /// <param name="httpMethod">HTTP method to use for the request</param>
        /// <param name="json">Optional json to send with the request</param>
        /// <param name="deserializeFunc">Func to handle deserialization of data when the request goes well</param>
        /// <returns>A deserialized entity with data when things go well, exceptionns otherwise</returns>
        private async Task<T> SendRequestAsync<T>(string absoluteUriPath, HttpMethod httpMethod, string json, Func<string, T> deserializeFunc)
            where T : IHubSpotEntity, new()
        {
            var fullUrl = $"{HubSpotBaseUrl}{absoluteUriPath}?hapikey={_apiKey}";
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
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new NotImplementedException("Deal with non success codes somehow!");
            }

            return deserializeFunc(responseData);
        }
    }
}