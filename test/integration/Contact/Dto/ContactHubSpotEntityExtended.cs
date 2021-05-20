using System;
using System.Runtime.Serialization;
using Skarp.HubSpotClient.Contact.Dto;

namespace integration.Contact.Dto
{
    /// <summary>
    /// Extended HubSpot Contact Entity
    /// </summary>
    /// <remarks>Used to test functionality of DateTime/DateTime?/DateTimeOffset/DateTimeOffset?</remarks>
    [DataContract]
    public class ContactHubSpotEntityExtended : ContactHubSpotEntity
    {
        [DataMember(Name = "lastmodifieddate")]
        public DateTimeOffset? LastModified { get; set; }
    }
}
