using Newtonsoft.Json;
using Skarp.HubSpotClient.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Skarp.HubSpotClient.ListOfContacts.Dto
{
    public class ListOfContactListsHubSpotEntity: IHubSpotEntity
    {
        [DataContract(Name="MetaData")]
        public class ContactListsMetaData
        {
            [DataMember(Name="processing")]
            public string Processing { get; set; }
            [DataMember(Name="size")]
            public int Size { get; set; }
            [DataMember(Name="error")]
            public string Error { get; set; }
            [DataMember(Name="lastProcessingStateChangeAt")]
            public long LastProcessingStateChangeAtTimeStamp { get; set; }
            [DataMember(Name="lastSizeChangeAt")]
            public long LastSizeChangeAtTimeStamp { get; set; }
        }

        [DataContract(Name="List")]
        public class ContactListsItem
        {
            [DataMember(Name="dynamic")]
            public bool Dynamic { get; set; }
            [DataMember(Name="metaData")]  
            public ContactListsMetaData MetaData { get; set; }
            [DataMember(Name="name")]  
            public string Name { get; set; }
            [DataMember(Name="filters")] 
            public List<List<ContactListsFilter>> Filters { get; set; }
            [DataMember(Name="portalId")] 
            public int PortalId { get; set; }
            [DataMember(Name="createdAt")] 
            public long CreatedAtTimeStamp { get; set; }
            [DataMember(Name="listId")] 
            public int ListId { get; set; }
            [DataMember(Name="updatedAt")] 
            public long UpdatedAtTimeStamp { get; set; }
            [DataMember(Name="listType")] 
            public string ListType { get; set; }
            [DataMember(Name="internalListId")] 
            public int InternalListId { get; set; }
            [DataMember(Name="deleteable")] 
            public bool Deleteable { get; set; }
        }

        [DataContract(Name="filter")]
        public class ContactListsFilter
         {
            [DataMember(Name="filterFamily")] 
            public string FilterFamily { get; set; }
            [DataMember(Name="withinTimeMode")] 
            public string WithinTimeMode { get; set; }
            [DataMember(Name="checkPastVersions")] 
            public bool CheckPastVersions { get; set; }
            [DataMember(Name="type")] 
            public string Type { get; set; }
            [DataMember(Name="property")] 
            public string Property { get; set; }
            [DataMember(Name="value")] 
            public string Value { get; set; }
            [DataMember(Name = "operator")]
            public string op { get; set; }
        }


        public List<ContactListsItem> lists { get; set; }
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
