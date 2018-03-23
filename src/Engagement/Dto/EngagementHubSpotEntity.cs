using System.Collections.Generic;
using System.Runtime.Serialization;
using Skarp.HubSpotClient.Engagement.Interfaces;

namespace Skarp.HubSpotClient.Engagement.Dto
{
    [DataContract]
    public class EngagementHubSpotEntity : IEngagementHubSpotEntity
    {
        /// <summary>
        /// Contacts unique Id in HubSpot
        /// </summary>
        [DataMember(Name = "vid")]
        [IgnoreDataMember]
        public long? Id { get; set; }

        [DataMember(Name = "engagement")]
        public Engagement Engagement { get; set; }

        [DataMember(Name = "associations")]
        public EngagementAssociations Associations { get; set; }

        [DataMember(Name = "attachments")]
        public List<EngagementAttachment> Attachments { get; set; } = new List<EngagementAttachment>();

        [DataMember(Name = "metadata")]
        public EngagementMetaData MetaData { get; set; }

        public string RouteBasePath => "/engagements/v1";
        public bool IsNameValue => false;
        public virtual void ToHubSpotDataEntity(ref dynamic converted)
        {

        }

        public virtual void FromHubSpotDataEntity(dynamic hubspotData)
        {

        }
    }
}