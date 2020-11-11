using Skarp.HubSpotClient.Owner.Interfaces;
using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.Owner.Dto
{
    [DataContract]
    public class OwnerHubSpotEntity : IOwnerHubSpotEntity
    {
        [DataMember(Name = "ownerId")]
        public long Id { get; set; }
        [DataMember(Name = "portalId")]
        public long PortalId { get; set; }
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "lastName")]
        public string LastName { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }

        public string RouteBasePath => "/owners/v2";
    }
}
