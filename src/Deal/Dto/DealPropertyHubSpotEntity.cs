using System.Runtime.Serialization;
using Skarp.HubSpotClient.Common.Dto.Properties;
using Skarp.HubSpotClient.Deal.Interfaces;

namespace Skarp.HubSpotClient.Deal.Dto
{
    /// <summary>
    /// Deal Property
    /// </summary>
    /// <remarks><see href="https://legacydocs.hubspot.com/docs/methods/deals/get_deal_properties">Documentation</see></remarks>
    [DataContract]
    public class DealPropertyHubSpotEntity : PropertyHubSpotEntity, IDealPropertyHubSpotEntity
    {
        [DataMember(Name = "displayOrder")]
        public long DisplayOrder { get; set; }

        [DataMember(Name = "readOnlyValue")]
        public bool ReadOnlyValue { get; set; }

        [DataMember(Name = "readOnlyDefinition")]
        public bool ReadOnlyDefinition { get; set; }

        [DataMember(Name = "hidden")]
        public bool Hidden { get; set; }

        [DataMember(Name = "mutableDefinitionNotDeletable")]
        public bool MutableDefinitionNotDeletable { get; set; }

        [DataMember(Name = "calculated")]
        public bool Calculated { get; set; }

        [DataMember(Name = "externalOptions")]
        public bool ExternalOptions { get; set; }

        [DataMember(Name = "displayMode")]
        public string DisplayMode { get; set; }

        [DataMember(Name = "hubspotDefined")]
        public bool? HubSpotDefined { get; set; }
    }
}
