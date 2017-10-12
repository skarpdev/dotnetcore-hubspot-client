using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.Core.Requests
{
    public class RequestSerializer
    {
        private readonly RequestDataConverter _requestDataConverter;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestSerializer"/> class.
        /// </summary>
        protected RequestSerializer()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestSerializer"/> class.
        /// </summary>
        /// <remarks>Use this constructor if you wish to override dependencies</remarks>
        /// <param name="requestDataConverter">The request data converter.</param>
        public RequestSerializer(
            RequestDataConverter requestDataConverter) : this()
        {
            _requestDataConverter = requestDataConverter;
        }

        /// <summary>
        /// Serializes the entity to JSON.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The serialized entity</returns>
        public virtual string SerializeEntity(object obj)
        {
            if (obj is IHubSpotEntity entity)
            {
                var converted = _requestDataConverter.ToHubspotDataEntity(entity);

                entity.ToHubSpotDataEntity(ref converted);

                return JsonConvert.SerializeObject(
                    converted,
                    _jsonSerializerSettings);
            }

            return JsonConvert.SerializeObject(
                obj,
                _jsonSerializerSettings);
        }

        /// <summary>
        /// Deserialize the given JSON into a <see cref="IHubSpotEntity"/>
        /// </summary>
        /// <param name="json">The json data returned by HubSpot that should be converted</param>
        /// <returns>The deserialized entity</returns>
        public virtual IHubSpotEntity DeserializeEntity<T>(string json) where T : IHubSpotEntity, new()
        {
            var jobj = JsonConvert.DeserializeObject<ExpandoObject>(json);
            var converted = _requestDataConverter.FromHubSpotResponse<T>(jobj);

            converted.FromHubSpotDataEntity(jobj);

            return converted;
        }

        /// <summary>
        /// Deserialize the given JSON from a List requet into a <see cref="IHubSpotEntity"/>
        /// </summary>
        /// <param name="json">The JSON data returned from a List request to HubSpot</param>
        /// <returns></returns>
        public virtual IHubSpotEntity DeserializeListEntity<T>(string json) where T : IHubSpotEntity, new()
        {
            var expandoObject = JsonConvert.DeserializeObject<ExpandoObject>(json);
            var converted = _requestDataConverter.FromHubSpotListResponse<T>(expandoObject);
            return converted;
        }
    }
}
