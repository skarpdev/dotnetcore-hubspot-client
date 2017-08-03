namespace Skarp.HubSpotClient.Interfaces
{
    public interface IContactHubSpotEntity : IHubSpotEntity
    {
        long? Vid { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string Lastname { get; set; }
        string Website { get; set; }
        string Company { get; set; }
        string Phone { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string State { get; set; }
        string ZipCode { get; set; }
    }
}
