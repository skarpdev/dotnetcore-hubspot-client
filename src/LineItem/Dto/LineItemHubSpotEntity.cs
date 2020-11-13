using Skarp.HubSpotClient.LineItem.Interfaces;
using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.LineItem.Dto
{
    [DataContract]
    public class LineItemHubSpotEntity : ILineItemHubSpotEntity
    {
        [DataMember(Name = "objectId")]
        [IgnoreDataMember]
        public long? Id { get; set; }
        [DataMember(Name = "hs_product_id")]
        public string ProductId { get; set; }
        [DataMember(Name = "quantity")]
        public int Quantity { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        public string RouteBasePath => "/crm-objects/v1/objects";

        public bool IsNameValue => true;

        public virtual void FromHubSpotDataEntity(dynamic hubspotData)
        {
        }

        public virtual void ToHubSpotDataEntity(ref dynamic dataEntity)
        {
        }
    }
}
