using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.CustomObjects.Interfaces;

public interface ICustomObjectHubSpotEntity : IHubSpotEntity
{
    long? Id { get; set; }
    /// <summary>
    /// The Id Hubspot gives to your custom object when you create it for the first time
    /// </summary>
    string ObjectTypeId { get; set; }
    string RouteBasePath { get; }
}