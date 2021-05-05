using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.ListOfContacts.Dto
{
    [DataContract(Name = "MetaData")]
    public class ContactListMetaData
    {
        [DataMember(Name = "processing")]
        public string Processing { get; set; }
        [DataMember(Name = "size")]
        public int Size { get; set; }
        [DataMember(Name = "error")]
        public string Error { get; set; }
        [DataMember(Name = "lastProcessingStateChangeAt")]
        public long LastProcessingStateChangeAtTimeStamp { get; set; }
        [DataMember(Name = "lastSizeChangeAt")]
        public long LastSizeChangeAtTimeStamp { get; set; }
    }
}
