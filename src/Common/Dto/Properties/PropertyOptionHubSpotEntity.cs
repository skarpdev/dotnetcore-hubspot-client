using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.Common.Dto.Properties
{
    [DataContract]
    public class PropertyOptionHubSpotEntity
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

        [DataMember(Name = "displayOrder")]
        public long DisplayOrder { get; set; }

        [DataMember(Name = "doubleData")]
        public object DoubleData { get; set; }
    }
}
