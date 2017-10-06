namespace Skarp.HubSpotClient.Core.Interfaces
{
    public interface IHubSpotEntity
    {
        bool IsNameValue { get; }

        void AcceptHubSpotDataEntity(ref dynamic dataEntity);
    }
}
