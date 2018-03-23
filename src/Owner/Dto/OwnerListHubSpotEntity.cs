
using System.Collections.Generic;
using System.Runtime.Serialization;
using Skarp.HubSpotClient.Owner.Interfaces;

namespace Skarp.HubSpotClient.Owner.Dto
{
    [DataContract]
    public class OwnerListHubSpotEntity<T> : IOwnerListHubSpotEntity<T> where T : IOwnerHubSpotEntity
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        /// <value>
        /// The contacts.
        /// </value>
        [DataMember]
        public List<T> Owners { get; set; } = new List<T>();

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