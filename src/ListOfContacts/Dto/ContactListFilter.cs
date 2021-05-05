using Skarp.HubSpotClient.ListOfContacts.Interfaces;
using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.ListOfContacts.Dto
{

    [DataContract(Name = "filter")]
    public class ContactListFilter : IContactListFilter
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "property")]
        public string Property { get; set; }
        [DataMember(Name = "value")]
        public string Value { get; set; }
        [DataMember(Name = "operator")]
        public string op { get; set; }
    }

}
