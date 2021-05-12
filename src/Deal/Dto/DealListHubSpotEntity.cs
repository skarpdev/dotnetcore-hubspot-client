using System.Collections.Generic;
using System.Runtime.Serialization;
using Skarp.HubSpotClient.Deal.Interfaces;

namespace Skarp.HubSpotClient.Deal.Dto
{
    [DataContract]
    public class DealListHubSpotEntity<T> : IDealListHubSpotEntity<T> where T : IDealHubSpotEntity
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the deals.
        /// </summary>
        /// <value>
        /// The deals.
        /// </value>
        [DataMember(Name = "deals")]
        public IList<T> Deals { get; set; } = new List<T>();

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
        /// This is a mapping of the "offset" prop in the JSON return data from HubSpot
        /// </remarks>
        [DataMember(Name = "offset")]
        public long ContinuationOffset { get; set; }

        public string RouteBasePath => "/deals/v1";

        public bool IsNameValue => false;

        public List<string> PropertiesToInclude { get; set; } = new List<string>();

        public virtual void ToHubSpotDataEntity(ref dynamic converted)
        {

        }

        public virtual void FromHubSpotDataEntity(dynamic hubspotData)
        {

        }
    }
}
