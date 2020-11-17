using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.Deal.Dto
{
    [DataContract]
    public class DealHubSpotAssociations
    {

        [DataMember(Name = "associatedCompanyIds")]
        public long[] AssociatedCompany { get; set; }

        [DataMember(Name = "associatedVids")]
        public long[] AssociatedContacts { get; set; }
    }
}