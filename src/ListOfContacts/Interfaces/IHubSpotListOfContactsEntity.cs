using System.Collections.Generic;
using Skarp.HubSpotClient.Contact.Interfaces;
using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.ListOfContacts.Interfaces
{
    public interface IHubSpotListOfContactsEntity
    {
        /// <summary>
        /// Gets or sets the contact vids.
        /// </summary>
        /// <value>
        /// The contact vids.
        /// </value>
        IList<long> Vids { get; set; }
    }
}