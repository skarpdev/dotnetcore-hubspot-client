using System.Collections.Generic;

namespace Skarp.HubSpotClient.Requests
{
    /// <summary>
    /// Class that represents the input data model for hubspot API endpoints
    /// </summary>
    internal class HubspotDataEntity
    {
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public List<HubspotDataEntityProp> Properties { get; protected set; } = new List<HubspotDataEntityProp>();
    }
}