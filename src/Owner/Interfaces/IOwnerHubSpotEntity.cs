namespace Skarp.HubSpotClient.Owner.Interfaces
{
    // Not using IHubSpotEntity because the api does not map to and from with property values. Owner is a simple object structure.
    public interface IOwnerHubSpotEntity
    {
        long Id { get; set; }
        long PortalId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string RouteBasePath { get; }
    }
}
