using System.Collections.Generic;
using Skarp.HubSpotClient.Contact.Interfaces;
using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.ListOfContacts.Interfaces
{
    public interface IListOfContactsHubSpotEntity<T> : IHubSpotEntity where T : IContactHubSpotEntity
    {
        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        /// <value>
        /// The contacts.
        /// </value>
        IList<T> Contacts { get; set; }

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

        string RouteBasePath { get; }
    }
}