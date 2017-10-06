using System;
using System.Collections.Generic;
using System.Text;
using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.Company.Interfaces
{
    public interface ICompanySearchResultEntity<T> : IHubSpotEntity where T : ICompanyHubSpotEntity
    {
        IList<T> Results { get; set; }

        bool MoreResultsAvailable { get; set; }

        CompanySearchOffset Offset { get; set; }
    }
}
