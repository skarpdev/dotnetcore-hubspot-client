using System.Dynamic;

namespace Skarp.HubSpotClient.Core.Interfaces
{
    public interface IHubSpotEntity
    {
        bool IsNameValue { get; }

        void ToHubSpotDataEntity(ref dynamic dataEntity);

        void FromHubSpotDataEntity(dynamic hubspotData);
    }
}
