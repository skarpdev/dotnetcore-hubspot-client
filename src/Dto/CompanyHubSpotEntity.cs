using System;
using System.Collections.Generic;
using System.Text;
using Skarp.HubSpotClient.Interfaces;

namespace Skarp.HubSpotClient.Dto
{
    public class CompanyHubSpotEntity : ICompanyHubSpotEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string RoutePath => "/companies/v2/companies/";
    }
}
