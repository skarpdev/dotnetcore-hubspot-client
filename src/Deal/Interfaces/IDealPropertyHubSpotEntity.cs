using Skarp.HubSpotClient.Common.Interfaces;

namespace Skarp.HubSpotClient.Deal.Interfaces
{
    public interface IDealPropertyHubSpotEntity : IPropertyHubSpotEntity
    {
        long DisplayOrder { get; set; }

        bool ReadOnlyValue { get; set; }

        bool ReadOnlyDefinition { get; set; }

        bool Hidden { get; set; }

        bool MutableDefinitionNotDeletable { get; set; }

        bool Calculated { get; set; }

        bool ExternalOptions { get; set; }

        string DisplayMode { get; set; }

        bool? HubSpotDefined { get; set; }
    }
}
