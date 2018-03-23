using System.Collections.Generic;
using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.Owner.Interfaces
{
    public interface IOwnerListHubSpotEntity<T> : IHubSpotEntity, IHubSpotReturnListEntity where T : IOwnerHubSpotEntity
    {
        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        /// <value>
        /// The contacts.
        /// </value>
        List<T> Owners { get; set; }
    }
}