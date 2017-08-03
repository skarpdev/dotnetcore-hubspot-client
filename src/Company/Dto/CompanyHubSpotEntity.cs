using Skarp.HubSpotClient.Company.Interfaces;

namespace Skarp.HubSpotClient.Company.Dto
{
    public class CompanyHubSpotEntity : ICompanyHubSpotEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string RouteBasePath => "/companies/v2";
    }
}
