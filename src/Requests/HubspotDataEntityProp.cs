namespace Skarp.HubSpotClient.Requests
{
    /// <summary>
    /// Represents a property value instance that can be sent to HubSpot
    /// </summary>
    internal class HubspotDataEntityProp
    {
        public string Property { get; set; }
        
        public string Value { get; set; }
    }
}