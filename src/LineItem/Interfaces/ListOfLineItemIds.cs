using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.LineItem.Interfaces
{
    public class ListOfLineItemIds
    {
        [DataMember(Name = "ids")]
        public IList<long> Ids { get; set; } = new List<long>();
    }
}
