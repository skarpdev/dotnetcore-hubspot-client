using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.LineItem.Interfaces
{
    public interface ILineItemHubSpotEntity : IHubSpotEntity
    {
        long? Id { get; set; }
        string ProductId { get; set; }
        int Quantity { get; set; }
        string Name { get; set; }
        decimal Price { get; set; }
        string RouteBasePath { get; }
    }
}
