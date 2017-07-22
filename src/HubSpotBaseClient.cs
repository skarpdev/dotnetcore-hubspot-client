using System;
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

        protected async Task<T> PostAsync<T>(string absoluteUriPath, IHubSpotEntity entity) where T : IHubSpotEntity, new()
        {
            Logger.LogDebug("Post async for uri path: '{0}' with type: '{1}'", absoluteUriPath, entity.GetType().Name);

            var fullUrl = $"{HubSpotBaseUrl}{absoluteUriPath}?hapikey={_apiKey}";
            Logger.LogDebug("Full url: '{0}'", fullUrl);

            var json = _serializer.SerializeEntity(entity);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(fullUrl),
                Content = new JsonContent(json)

            };
            var response = await HttpClient.SendAsync(request);
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new NotImplementedException("Deal with non success codes somehow!");
            }

            var data = (T)_serializer.DeserializeEntity<T>(responseData);
            return data;
        }
    }
}