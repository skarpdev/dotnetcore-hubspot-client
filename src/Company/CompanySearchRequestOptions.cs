using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.Company
{
    [DataContract]
    public class CompanySearchRequestOptions
    {
        [DataMember(Name = "properties")]
        public List<string> Properties { get; set; } = new List<string> { "domain", "name", "website" };
    }
}