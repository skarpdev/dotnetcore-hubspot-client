using System.Collections.Generic;

namespace Skarp.HubSpotClient.LineItem
{
    public class LineItemListRequestOptions
    {
        /// <summary>
        /// Get or set the continuation offset when calling list many times to enumerate all your line items.
        /// The API will return up to 100 results per page.
        /// </summary>
        /// <remarks>
        /// The return DTO from List contains the current "offset" that you can inject into your next list call 
        /// to continue the listing process
        /// </remarks>
        public long? Offset { get; set; } = null;
        public List<string> PropertiesToInclude { get; set; } = new List<string>();
    }

    public class LineItemGetRequestOptions
    {
        public List<string> PropertiesToInclude { get; set; } = new List<string>();
        public bool IncludeDeletes { get; set; }
    }
}
