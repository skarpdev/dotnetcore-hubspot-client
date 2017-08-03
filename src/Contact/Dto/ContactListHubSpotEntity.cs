using System.Collections.Generic;
using System.Runtime.Serialization;
using Skarp.HubSpotClient.Contact.Interfaces;

namespace Skarp.HubSpotClient.Contact.Dto
{
    [DataContract]
    public class ContactListHubSpotEntity<T> : IContactListHubSpotEntity<T> where T : IContactHubSpotEntity
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        /// <value>
        /// The contacts.
        /// </value>
        [DataMember(Name = "contacts")]
        public IList<T> Contacts { get; set; } = new List<T>();

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether more results are available.
        /// </summary>
        /// <value>
        /// <c>true</c> if [more results available]; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This is a mapping of the "has-more" prop in the JSON return data from HubSpot
        /// </remarks>
        [DataMember(Name = "has-more")]
        public bool MoreResultsAvailable { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the continuation offset.
        /// </summary>
        /// <value>
        /// The continuation offset.
        /// </value>
        /// <remarks>
        /// This is a mapping of the "vid-offset" prop in the JSON reeturn data from HubSpot
        /// </remarks>
        [DataMember(Name = "vid-offset")]
        public long ContinuationOffset { get; set; }

        public string RouteBasePath => "/contacts/v1";
    }
}
