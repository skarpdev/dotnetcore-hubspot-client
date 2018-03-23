using System.Runtime.Serialization;
using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.Engagement.Dto
{
    [DataContract]
    public class Engagement : IHubSpotEntity
    {
        [DataMember(Name = "active")]
        public bool Active { get; set; } = true;

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "timestamp")]
        public long Timestamp { get; set; }

        [DataMember(Name = "createdBy")]
        public long CreatedById { get; set; }

        [DataMember(Name = "modifiedBy")]
        public long ModifiedById { get; set; }

        [DataMember(Name = "ownerId")]
        public long OwnerId { get; set; }

        public string RouteBasePath => "";
        public bool IsNameValue => false;
        public virtual void ToHubSpotDataEntity(ref dynamic converted)
        {

        }

        public virtual void FromHubSpotDataEntity(dynamic hubspotData)
        {

        }
    }
}