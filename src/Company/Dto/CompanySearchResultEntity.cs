using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Skarp.HubSpotClient.Company.Interfaces;
using Skarp.HubSpotClient.Contact.Interfaces;

namespace Skarp.HubSpotClient.Company.Dto
{
    [DataContract]
    public class CompanySearchResultEntity<T> : ICompanySearchResultEntity<T> where T : ICompanyHubSpotEntity
    {
        [DataMember(Name = "results")]
        public IList<T> Results { get; set; }

        [DataMember(Name = "hasMore")]
        public bool MoreResultsAvailable { get; set; }

        [DataMember(Name="offset")]
        public CompanySearchOffset Offset { get; set; }

        public bool IsNameValue => false;

        public void ToHubSpotDataEntity(ref dynamic dataEntity)
        {
        }

        public void FromHubSpotDataEntity(dynamic hubspotData)
        {
            
        }
    }
}