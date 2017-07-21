using System.Reflection;
using Microsoft.Extensions.Logging;
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
                if(item.Value == null) continue;

                mapped.Properties.Add(item);
            }

            _logger.LogDebug("Mapping complete, returning data");
            return mapped;
        }
    }
}
