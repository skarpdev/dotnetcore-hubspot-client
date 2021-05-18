using System.Collections.Generic;
using System.Runtime.Serialization;
using Skarp.HubSpotClient.Common.Interfaces;

namespace Skarp.HubSpotClient.Common.Dto.Properties
{
    [DataContract]
    public class PropertyHubSpotEntity : IPropertyHubSpotEntity
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "groupName")]
        public string GroupName { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "fieldType")]
        public string FieldType { get; set; }

        [DataMember(Name = "options")]
        public List<PropertyOptionHubSpotEntity> Options { get; set; }

        public bool IsNameValue => false;

        public string RouteBasePath => "/properties/v1";

        public virtual void ToHubSpotDataEntity(ref dynamic dataEntity)
        {
            
        }

        public virtual void FromHubSpotDataEntity(dynamic hubspotData)
        {
        }
    }
}
