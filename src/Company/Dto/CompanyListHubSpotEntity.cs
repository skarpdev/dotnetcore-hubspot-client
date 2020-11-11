using Skarp.HubSpotClient.Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Skarp.HubSpotClient.Company.Dto
{
        [DataContract]
    public class CompanyListHubSpotEntity<T> : ICompanyListHubSpotEntity<T> where T : ICompanyHubSpotEntity
    {
            /// <inheritdoc />
            /// <summary>
            /// Gets or sets the companies.
            /// </summary>
            /// <value>
            /// The companies.
            /// </value>
            [DataMember(Name = "companies")]
            public IList<T> Companies { get; set; } = new List<T>();

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
            /// This is a mapping of the "offset" prop in the JSON reeturn data from HubSpot
            /// </remarks>
            [DataMember(Name = "offset")]
            public long ContinuationOffset { get; set; }

            public string RouteBasePath => "/companies/v2";

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
