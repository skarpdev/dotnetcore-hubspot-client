using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.Owner.Dto
{
    [DataContract(Name = "RemoteList")]
    public class OwnerRemoteList
    {
        public int id { get; set; }
        public int portalId { get; set; }
        public int ownerId { get; set; }
        public string remoteId { get; set; }
        public string remoteType { get; set; }
        public bool active { get; set; }
    }
}