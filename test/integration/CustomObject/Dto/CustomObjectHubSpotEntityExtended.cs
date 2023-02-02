using Skarp.HubSpotClient.CustomObjects.Dto;

namespace integration.CustomObject.Dto;

/// <summary>
/// Create
/// </summary>
public class CustomObjectHubSpotEntityExtended : CustomObjectHubSpotEntity
{
    public CustomObjectHubSpotEntityExtended()
    {
        ObjectTypeId = "2-6013641";
    }

    public string my_object_property { get; set; }
}