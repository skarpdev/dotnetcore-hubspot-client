using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.Owner.Interfaces
{
    public interface IOwnerHubSpotEntity : IHubSpotEntity
    {
        long? Id { get; set; }
        string RouteBasePath { get; }
    }
}