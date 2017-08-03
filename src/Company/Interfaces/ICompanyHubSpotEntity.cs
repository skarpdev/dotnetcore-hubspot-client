using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.Company.Interfaces
{
    public interface ICompanyHubSpotEntity : IHubSpotEntity
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
