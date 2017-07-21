using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Skarp.HubSpotClient.Interfaces;

namespace Skarp.HubSpotClient.Requests
{
    public class RequestDataConverter
    {
        private readonly ILogger<RequestDataConverter> _logger;

        public RequestDataConverter(
            ILogger<RequestDataConverter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Converts the given <paramref name="entity"/> to a hubspot data entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public HubspotDataEntity ToHubspotDataEntity(IHubSpotEntity entity)
        {
            _logger.LogDebug("Convert ToHubspotDataEntity");
            var mapped = new HubspotDataEntity();
            bool isv2Route = entity.RouteBasePath.Contains("/v2");
            _logger.LogDebug("isv2route: {0}", isv2Route);

            var allProps = entity.GetType().GetProperties();
            _logger.LogDebug("Have {0} props to map", allProps.Length);

            foreach (var prop in allProps)
            {
                _logger.LogDebug("Mapping prop: '{0}'", prop.Name);
                if (prop.Name.Equals("RouteBasePath")) continue;

                var propValue = prop.GetValue(entity);
                var item = new HubspotDataEntityProp
                {
                    Property = prop.Name,
                    Value = propValue?.ToString()
                };

                if (isv2Route)
                {
                    item.Property = null;
                    item.Name = prop.Name;
                }
                if (item.Value == null) continue;

                mapped.Properties.Add(item);
            }

            _logger.LogDebug("Mapping complete, returning data");
            return mapped;
        }

        /// <summary>
        /// Convert from the dynamicly typed <see cref="ExpandoObject"/> into a strongly typed <see cref="IHubSpotEntity"/>
        /// </summary>
        /// <param name="dynamicObject">The <see cref="ExpandoObject"/> representation of the returned json</param>
        /// <returns></returns>
        public T FromHubSpotResponse<T>(ExpandoObject dynamicObject) where T : IHubSpotEntity, new()
        {
            var expandoDict = (IDictionary<string, object>) dynamicObject;

            var data = new T();
            var dataProps = data.GetType().GetProperties();

            if (expandoDict.TryGetValue("vid", out var vidData))
            {
                var vidProp = dataProps.SingleOrDefault(q => q.Name.ToLowerInvariant() == "vid");
                vidProp?.SetValue(data, vidData);
            }

            if (!expandoDict.TryGetValue("properties", out var dynamicProperties))
            {
                throw new ArgumentException("The given JSON document does not contain a properties object");
            }
            
            foreach (var dynamicProp in dynamicProperties as ExpandoObject)
            {
                // prop.Key contains the name of the property we wish to map into the DTO
                // prop.Value contains the data returned by HubSpot, which is also an object 
                // in there we need to go get the "value" prop to get the actual value
                _logger.LogDebug("Looking at dynamic prop: {0}", dynamicProp.Key);
                
                if (!((IDictionary<string, Object>) dynamicProp.Value).TryGetValue("value", out object dynamicValue))
                {
                    continue;
                }
                
                var targetProp =
                    dataProps.SingleOrDefault(q => q.Name.ToLowerInvariant() == dynamicProp.Key.ToLowerInvariant());
                _logger.LogDebug("Have target prop? '{0}' with name: '{1}' and actual value: '{2}'", targetProp != null, targetProp?.Name ?? "N/A", dynamicValue);
                targetProp?.SetValue(data, dynamicValue);
            }
            return data;
        }
    }
}