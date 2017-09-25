using System.Runtime.Serialization;
using Skarp.HubSpotClient.Contact.Interfaces;

namespace Skarp.HubSpotClient.Contact.Dto
{
    [DataContract]
    public class ContactHubSpotEntity : IContactHubSpotEntity
    {
        /// <summary>
        /// Contacts unique Id in HubSpot
        /// </summary>
        [DataMember(Name = "vid")]
        [IgnoreDataMember]
        public long? Id { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "firstname")]
        public string FirstName { get; set; }
        [DataMember(Name = "lastname")]
        public string Lastname { get; set; }
        [DataMember(Name = "website")]
        public string Website { get; set; }
        [DataMember(Name = "company")]
        public string Company { get; set; }
        [DataMember(Name = "phone")]
        public string Phone { get; set; }
        [DataMember(Name = "address")]
        public string Address { get; set; }
        [DataMember(Name = "city")]
        public string City { get; set; }
        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "zipcode")]
        public string ZipCode { get; set; }

        public string RouteBasePath => "/contacts/v1";
        public bool IsNameValue => false;
        public virtual void AcceptHubSpotDataEntity(ref dynamic converted)
        {

        }
    }
}
