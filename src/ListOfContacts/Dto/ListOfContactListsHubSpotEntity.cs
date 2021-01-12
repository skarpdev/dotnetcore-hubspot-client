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
        [DataContract]
        public class MetaData
        {
            public string processing { get; set; }
            public int size { get; set; }
            public string error { get; set; }
            public DateTime lastProcessingStateChangeAt { get; set; }
            public DateTime lastSizeChangeAt { get; set; }
        }

        [DataContract]
        public class List
        {
            public bool dynamic { get; set; }
            public MetaData metaData { get; set; }
            public string name { get; set; }
            public List<List<filter>> filters { get; set; }
            public int portalId { get; set; }
            public DateTime createdAt { get; set; }
            public int listId { get; set; }
            public DateTime updatedAt { get; set; }
            public string listType { get; set; }
            public int internalListId { get; set; }
            public bool deleteable { get; set; }
        }

        [DataContract]
        public class filter
         {
            public string filterFamily { get; set; }
            public string withinTimeMode { get; set; }
            public bool checkPastVersions { get; set; }
            public string type { get; set; }
            public string property { get; set; }
            public string value { get; set; }

            [DataMember(Name = "operator")]
            public string op { get; set; }
        }


        public List<List> lists { get; set; }
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
