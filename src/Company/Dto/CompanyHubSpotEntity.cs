using Skarp.HubSpotClient.Company.Interfaces;
using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.Company.Dto
{
    [DataContract]
    public class CompanyHubSpotEntity : ICompanyHubSpotEntity
    {
        [DataMember(Name = "companyId")]
        [IgnoreDataMember]
        public long? Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }

        public string RouteBasePath => "/companies/v2";
    }
}
