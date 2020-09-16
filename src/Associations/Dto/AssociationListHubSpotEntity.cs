using Skarp.HubSpotClient.Associations.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Skarp.HubSpotClient.Associations.Dto
{
    [DataContract]
    public class AssociationListHubSpotEntity<T> : IAssociationListHubSpotEntity<T>
    {
        [DataMember(Name = "results")]
        public IList<T> Results { get; set; } = new List<T>();
        [DataMember(Name = "hasMore")]
        public bool MoreResultsAvailable { get; set; }
        [DataMember(Name = "offset")]
        public long ContinuationOffset { get; set; }
    }
}
