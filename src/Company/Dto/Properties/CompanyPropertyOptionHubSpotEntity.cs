using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.Company.Dto.Properties
{
    [DataContract]
    public class CompanyPropertyOptionHubSpotEntity
    {
        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "readOnly")]
        public bool? ReadOnly { get; set; }

        [DataMember(Name = "hidden")]
        public bool Hidden { get; set; }
    }
}
