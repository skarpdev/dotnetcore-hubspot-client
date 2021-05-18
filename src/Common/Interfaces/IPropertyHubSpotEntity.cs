using System.Collections.Generic;
using Skarp.HubSpotClient.Common.Dto.Properties;
using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.Common.Interfaces
{
    public interface IPropertyHubSpotEntity : IHubSpotEntity
    {
        string Name { get; set; }

        string Label { get; set; }

        string Description { get; set; }

        string GroupName { get; set; }

        string Type { get; set; }

        string FieldType { get; set; }

        string RouteBasePath { get; }

        List<PropertyOptionHubSpotEntity> Options { get; set; }
    }
}
