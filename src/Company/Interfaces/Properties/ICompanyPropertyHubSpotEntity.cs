using System.Collections.Generic;
using Skarp.HubSpotClient.Company.Dto.Properties;
using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.Company.Interfaces.Properties
{
    public interface ICompanyPropertyHubSpotEntity : IHubSpotEntity
    {
        string Name { get; set; }
        string Label { get; set; }
        string Description { get; set; }
        string GroupName { get; set; }
        string Type { get; set; }
        string FieldType { get; set; }
        List<CompanyPropertyOptionHubSpotEntity> Options { get; set; }
        string RouteBasePath { get; }
    }
}
