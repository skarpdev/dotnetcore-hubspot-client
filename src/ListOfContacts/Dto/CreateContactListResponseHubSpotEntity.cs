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
    public class CreateContactListResponseHubSpotEntity: IHubSpotEntity
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "internalListId")]
        public int InternalListId { get; set; }
        [DataMember(Name = "listId")]
        public int ListId { get; set; }
        [DataMember(Name = "deleted")]
        public bool Deleted { get; set; }
        [DataMember(Name = "dynamic")]
        public bool Dynamic { get; set; }
        [DataMember(Name = "portalId")]
        public int PortalId { get; set; }
        [DataMember(Name = "createdAt")]
        public string CreatedAtTimeStamp { get; set; }
        [DataMember(Name = "updatedAt")]
        public string UpdatedAtTimeStamp { get; set; }
        [DataMember(Name = "metaData")]
        public ContactListMetaData MetaData { get; set; }
        [DataMember(Name = "filters")]
        public List<List<ContactListFilter>> Filters { get; set; }
        [DataMember(Name = "listType")]
        public string ListType { get; set; }
        [DataMember(Name = "archived")]
        public bool Archived { get; set; }

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
