using Skarp.HubSpotClient.Associations.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Skarp.HubSpotClient.Associations.Dto
{
    [DataContract]
    public class AssociationHubSpotEntity : IAssociationHubSpotEntity
    {
        [DataMember(Name = "fromObjectId")]
        public long FromObjectId { get; set; }
        [DataMember(Name = "toObjectId")]
        public long ToObjectId { get; set; }

        [DataMember(Name = "category")]
        public string Category => "HUBSPOT_DEFINED";

        [DataMember(Name = "definitionId")]
        public int DefinitionId { get; set; }

        public string RouteBasePath => "/crm-associations/v1";
    }
}
