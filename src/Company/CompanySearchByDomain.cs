using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.Company
{
    [DataContract]
    public class CompanySearchByDomain
    {
        [DataMember(Name = "limit")]
        public int Limit { get; set; } = 100;

        [DataMember(Name = "requestOptions")]
        public CompanySearchRequestOptions RequestOptions { get; set; } = new CompanySearchRequestOptions();

        [DataMember(Name = "offset")]
        public CompanySearchOffset Offset { get; set; } = new CompanySearchOffset();

        public string RouteBasePath => "/companies/v2";
        public bool IsNameValue => true;
        public void AcceptHubSpotDataEntity(ref object converted)
        {

        }
    }
}