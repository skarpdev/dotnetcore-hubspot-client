using Newtonsoft.Json;
using Skarp.HubSpotClient.Core.Interfaces;
using Skarp.HubSpotClient.ListOfContacts.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Skarp.HubSpotClient.ListOfContacts.Dto
{
    [DataContract]
    public class CreateContactListRequestHubSpotEntity : ICreateContactListRequestHubSpotEntity
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "dynamic")]
        public bool? Dynamic { get; set; }
        [DataMember(Name = "portalId")]
        public int? PortalId { get; set; }
        [DataMember(Name = "filters")]
        public List<List<IContactListFilter>> Filters { get; set; }

        public int offset { get; set; }

        [DataMember(Name = "has-more")]
        public bool HasMore { get; set; }

        public bool IsNameValue => false;

        public virtual void ToHubSpotDataEntity(ref dynamic dataEntity)
        {

        }

        public virtual void FromHubSpotDataEntity(dynamic hubspotData)
        {

        }
    }
}
