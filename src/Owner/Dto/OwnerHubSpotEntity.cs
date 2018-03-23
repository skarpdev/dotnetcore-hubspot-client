using System.Collections.Generic;
using System.Runtime.Serialization;
using Skarp.HubSpotClient.Owner.Interfaces;

namespace Skarp.HubSpotClient.Owner.Dto
{
    [DataContract]
    public class OwnerHubSpotEntity : IOwnerHubSpotEntity
    {
        /// <summary>
        /// Owners unique Id in HubSpot
        /// </summary>
        [DataMember(Name = "ownerId")]
        [IgnoreDataMember]
        public long? Id { get; set; }

        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "createdAt")]
        public object CreatedAt { get; set; }

        [DataMember(Name = "updatedAt")]
        public object UpdatedAt { get; set; }
        
        //[DataMember(Name = "remoteList")]
        //[IgnoreDataMember]
        //public List<OwnerRemoteList> RemoteList { get; set; }

        [DataMember(Name = "hasContactsAccess")]
        public bool HasContactsAccess { get; set; }

        [DataMember(Name = "signature")]
        public string Signature { get; set; }

        public string RouteBasePath => "/owners/v2";
        public bool IsNameValue => false;

        public virtual void ToHubSpotDataEntity(ref dynamic converted)
        {

        }

        public virtual void FromHubSpotDataEntity(dynamic hubspotData)
        {

        }
    }
}