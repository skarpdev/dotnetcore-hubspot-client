using Skarp.HubSpotClient.Deal.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

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


    [DataContract]
    public class DealHubSpotEntity : IDealHubSpotEntity
    {
        /// <summary>
        /// Contacts unique Id in HubSpot
        /// </summary>
        [DataMember(Name = "dealId")]
        [IgnoreDataMember]
        public long? Id { get; set; }
        [DataMember(Name = "dealname")]
        public string Name { get; set; }
        [DataMember(Name = "dealstage")]
        public string Stage { get; set; }
        [DataMember(Name = "pipeline")]
        public string Pipeline { get; set; }
        [DataMember(Name = "hubspot_owner_id")]
        public string OwnerId { get; set; }
        [DataMember(Name = "closedate")]
        public string CloseDate { get; set; }
        [DataMember(Name = "amount")]
        public int Amount { get; set; }
        [DataMember(Name = "dealtype")]
        public string DealType { get; set; }


        [IgnoreDataMember]
        public DealHubSpotAssociations Associations { get; set; }
        public string RouteBasePath => "/deals/v1";
        public bool IsNameValue => true;
        public virtual void AcceptHubSpotDataEntity(ref dynamic converted)
        {
            converted.Associations = Associations;
        }
    }
}
