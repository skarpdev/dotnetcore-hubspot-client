using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.Company
{
    [DataContract]
    public class CompanySearchOffset
    {

        [DataMember(Name = "isPrimary")]
        public bool IsPrimary { get; set; } = true;

        [DataMember(Name = "companyId")]
        public long CompanyId { get; set; } = 0;
    }
}