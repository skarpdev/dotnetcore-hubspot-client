namespace Skarp.HubSpotClient.Interfaces
{
    public interface IHubSpotEntity
    {

        /// <summary>
        /// When implemented in a downstream class it should return the HubSpot route that the entity belongs to,
        /// e.g /companies/v2/companies/
        /// </summary>
        /// <returns></returns>
        string RouteBasePath { get;  }
    }
}
