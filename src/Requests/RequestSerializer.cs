using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Skarp.HubSpotClient.Interfaces;

namespace Skarp.HubSpotClient.Requests
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
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual string SerializeEntity(IHubSpotEntity entity)
        {
            var converted = _requestDataConverter.ToHubspotDataEntity(entity);
            
            return JsonConvert.SerializeObject(
                   converted,
                   _jsonSerializerSettings);
        }
    }
}
