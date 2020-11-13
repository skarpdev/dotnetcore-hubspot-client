using Skarp.HubSpotClient.Core.Interfaces;
using System.Collections.Generic;

namespace Skarp.HubSpotClient.LineItem.Interfaces
{
    public interface ILineItemListHubSpotEntity<T> : IHubSpotEntity where T : ILineItemHubSpotEntity
    {
        /// <summary>
        /// Gets or sets the line items.
        /// </summary>
        /// <value>
        /// The line items.
        /// </value>
        IList<T> LineItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether more results are available.
        /// </summary>
        /// <remarks>
        /// This is a mapping of the "hasMore" prop in the JSON return data from HubSpot
        /// </remarks>
        /// <value>
        ///   <c>true</c> if [more results available]; otherwise, <c>false</c>.
        /// </value>
        bool MoreResultsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the continuation offset.
        /// </summary>
        /// <remarks>
        /// This is a mapping of the "offset" prop in the JSON return data from HubSpot
        /// </remarks>
        /// <value>
        /// The continuation offset.
        /// </value>
        long ContinuationOffset { get; set; }

        string RouteBasePath { get; }
    }
}
