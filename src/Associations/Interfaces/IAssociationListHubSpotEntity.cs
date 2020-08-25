using Skarp.HubSpotClient.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skarp.HubSpotClient.Associations.Interfaces
{
    public interface IAssociationListHubSpotEntity<T>
    {
        /// <summary>
        /// Gets or sets the association results.
        /// </summary>
        /// <value>
        /// The contacts.
        /// </value>
        IList<T> Results { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether more results are available.
        /// </summary>
        /// <remarks>
        /// This is a mapping of the "has-more" prop in the JSON return data from HubSpot
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

    }
}
