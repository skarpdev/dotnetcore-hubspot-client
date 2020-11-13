using Skarp.HubSpotClient.LineItem.Interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.LineItem.Dto
{
    [DataContract]
    public class LineItemListHubSpotEntity<T> : ILineItemListHubSpotEntity<T> where T : ILineItemHubSpotEntity
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the line items.
        /// </summary>
        /// <value>
        /// The line items.
        /// </value>
        [DataMember(Name = "objects")]
        public IList<T> LineItems { get; set; } = new List<T>();

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether more results are available.
        /// </summary>
        /// <value>
        /// <c>true</c> if [more results available]; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This is a mapping of the "hasMore" prop in the JSON return data from HubSpot
        /// </remarks>
        [DataMember(Name = "hasMore")]
        public bool MoreResultsAvailable { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the continuation offset.
        /// </summary>
        /// <value>
        /// The continuation offset.
        /// </value>
        /// <remarks>
        /// This is a mapping of the "offset" prop in the JSON reeturn data from HubSpot
        /// </remarks>
        [DataMember(Name = "offset")]
        public long ContinuationOffset { get; set; }

        public string RouteBasePath => "/crm-objects/v1/objects";

        public bool IsNameValue => false;

        public virtual void ToHubSpotDataEntity(ref dynamic converted)
        {

        }

        public virtual void FromHubSpotDataEntity(dynamic hubspotData)
        {

        }
    }

}
