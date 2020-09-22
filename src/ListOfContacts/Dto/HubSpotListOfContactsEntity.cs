using System.Collections.Generic;
using System.Runtime.Serialization;
using Skarp.HubSpotClient.Contact.Interfaces;
using Skarp.HubSpotClient.ListOfContacts.Interfaces;

namespace Skarp.HubSpotClient.ListOfContacts.Dto
{
    [DataContract]
    public class HubSpotListOfContactsEntity : IHubSpotListOfContactsEntity
    {
       
        public string RouteBasePath => "/contacts/v1";

        public bool IsNameValue => false;

        [DataMember(Name = "vids")]
        public IList<long> Vids { get; set; } = new List<long>();

        public virtual void ToHubSpotDataEntity(ref dynamic converted)
        {

        }

        public virtual void FromHubSpotDataEntity(dynamic hubspotData)
        {
            
        }
    }
}
