using System;
using System.Collections.Generic;
using System.Text;

namespace Skarp.HubSpotClient.Interfaces
{
    public interface ICompanyHubSpotEntity : IHubSpotEntity
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
