using Skarp.HubSpotClient.Interfaces;

namespace Skarp.HubSpotClient.Dto
{
    public class ContactHubSpotEntity : IContactHubSpotEntity
    {
        /// <summary>
        /// Contacts unique Id in HubSpot
        /// </summary>
        public long? Vid { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Website { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        
        public string RouteBasePath => "/contacts/v1";
    }
}
