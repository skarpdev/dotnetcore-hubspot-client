using System.Collections.Generic;
using System.Dynamic;
using System.Text;
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
        /// Serializes entity list to JSON.
        /// </summary>
        /// <param name="entities">The list of entities.</param>
        /// <returns>The serialized list of entities</returns>
        public virtual string SerializeEntities<T>(List<T> objs)
        {
            var result = new StringBuilder("[");

            for (int i = 0; i < objs.Count; i++)
            {
                var obj = objs[i];
                if (obj is IHubSpotEntity entity)
                {
                    var converted = _requestDataConverter.ToHubspotDataEntity(entity);

                    entity.ToHubSpotDataEntity(ref converted);

                    result.Append(JsonConvert.SerializeObject(
                        converted,
                        _jsonSerializerSettings));
                }
                else
                {
                    result.Append(JsonConvert.SerializeObject(
                    obj,
                    _jsonSerializerSettings));
                }
                if ((i + 1) < objs.Count)
                {
                    result.Append(",");
                }
            }

            result.Append("]");
            return result.ToString();
        }

        /// <summary>
        /// Serializes entity list to name/value JSON.
        /// </summary>
        /// <param name="entities">The list of entities.</param>
        /// <returns>The serialized list of entities</returns>
        public virtual string SerializeEntitiesToNameValueList<T>(IList<T> objs)
        {
            var result = new StringBuilder("[");

            for (int i = 0; i < objs.Count; i++)
            {
                var obj = objs[i];

                var converted = _requestDataConverter.ToNameValueList(obj);

                result.Append(JsonConvert.SerializeObject(
                    converted,
                    _jsonSerializerSettings));

                if ((i + 1) < objs.Count)
                {
                    result.Append(",");
                }
            }
            result.Append("]");
            return result.ToString();
        }

        /// <summary>
        /// Serializes the entity as a name/value list to JSON.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The serialized entity</returns>
        public virtual string SerializeEntityToNameValueList<T>(T entity)
        {
            var converted = _requestDataConverter.ToNameValueList(entity);

            return JsonConvert.SerializeObject(
                converted,
                _jsonSerializerSettings);
        }

        /// <summary>
        /// Deserialize the given JSON
        /// </summary>
        /// <param name="json">The json data returned by HubSpot that should be converted</param>
        /// <returns>The deserialized entity</returns>
        public virtual T DeserializeGenericEntity<T>(string json)
        {
            var jobj = JsonConvert.DeserializeObject<T>(json);

            return jobj;
        }

        /// <summary>
        /// Deserialize the given JSON into a <see cref="IHubSpotEntity"/>
        /// </summary>
        /// <param name="json">The json data returned by HubSpot that should be converted</param>
        /// <returns>The deserialized entity</returns>
        public virtual T DeserializeEntity<T>(string json) where T : IHubSpotEntity, new()
        {
            var jobj = JsonConvert.DeserializeObject<ExpandoObject>(json);
            var converted = _requestDataConverter.FromHubSpotResponse<T>(jobj);

            converted.FromHubSpotDataEntity(jobj);

            return converted;
        }

        /// <summary>
        /// Deserialize the given JSON from a List request into a <see cref="IHubSpotEntity"/>
        /// </summary>
        /// <param name="json">The JSON data returned from a List request to HubSpot</param>
        /// <returns></returns>
        public virtual IHubSpotEntity DeserializeListEntity<T>(string json) where T : IHubSpotEntity, new()
        {
            var expandoObject = JsonConvert.DeserializeObject<ExpandoObject>(json);
            var converted = _requestDataConverter.FromHubSpotListResponse<T>(expandoObject);
            return converted;
        }

        /// <summary>
        /// Deserialize the given JSON into a list of <see cref="IHubSpotEntity"/>
        /// </summary>
        /// <param name="json">The JSON data returned from a HubSpot operation</param>
        /// <returns></returns>
        public virtual IEnumerable<T> DeserializeEntities<T>(string json) where T : IHubSpotEntity, new()
        {
            var listOfExpandoObjects = JsonConvert.DeserializeObject<IEnumerable<ExpandoObject>>(json);

            var convertedList = new List<T>();
            foreach (var expandoObject in listOfExpandoObjects)
            {
                convertedList.Add(_requestDataConverter.FromHubSpotResponse<T>(expandoObject));
            }
            return convertedList;
        }
    }
}
