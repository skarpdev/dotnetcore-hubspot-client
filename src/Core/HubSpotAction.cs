namespace Skarp.HubSpotClient.Core
{
    /// <summary>
    /// Enumerates the possible actions against the hubspot api
    /// </summary>
    public enum HubSpotAction
    {
        Create,

        Get,
        
        GetByEmail,

        List,

        Update,

        Delete,

        CreateOrUpdate,

        CreateBatch,

        DeleteBatch,

        UpdateBatch,

        ReadBatch
    }
}