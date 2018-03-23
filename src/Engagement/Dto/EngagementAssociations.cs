using System.Runtime.Serialization;
using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.Engagement.Dto
{
    [DataContract]
    public class EngagementAssociations : IHubSpotEntity
    {
        [DataMember(Name = "contactIds")]
        public long[] ContactIds { get; set; }

        [DataMember(Name = "companyIds")]
        public long[] CompanyIds { get; set; }

        [DataMember(Name = "dealIds")]
        public long[] DealIds { get; set; }

        [DataMember(Name = "ownerIds")]
        public long[] OwnerIds { get; set; }

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