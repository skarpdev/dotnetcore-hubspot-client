using System.Collections.Generic;
using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.Deal.Interfaces
{
    public interface IDealListHubSpotEntity<T> : IHubSpotEntity where T : IDealHubSpotEntity
    {
        /// <summary>
        /// Gets or sets the deals.
        /// </summary>
        /// <value>
        /// The deals.
        /// </value>
        IList<T> Deals { get; set; }

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
        /// This is a mapping of the "vid-offset" prop in the JSON reeturn data from HubSpot
        /// </remarks>
        /// <value>
        /// The continuation offset.
        /// </value>
        long ContinuationOffset { get; set; }

        string RouteBasePath { get; }

        List<string> PropertiesToInclude { get; set; }
    }
}
