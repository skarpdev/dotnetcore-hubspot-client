using System.Collections.Generic;

namespace Skarp.HubSpotClient.LineItem
{
    public class LineItemGetRequestOptions
    {
        public List<string> PropertiesToInclude { get; set; } = new List<string>();
        public List<string> PropertiesWithHistoryToInclude { get; set; } = new List<string>();
        public bool IncludeDeletes { get; set; }
    }
}
