using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.Engagement.Interfaces
{
    public interface IEngagementHubSpotEntity : IHubSpotEntity
    {
        long? Id { get; set; }
        string RouteBasePath { get; }
    }
}