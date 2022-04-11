using System.ComponentModel.DataAnnotations;
using Skarp.HubSpotClient.CustomObjects.Dto;

namespace integration.UniqueIdentifiers.Dto;

/// <summary>
/// Create
/// </summary>
public class UniqueIdentifierHubSpotEntityExtended : CustomObjectHubSpotEntity
{
    public UniqueIdentifierHubSpotEntityExtended()
    {
        ObjectTypeId = "2-6114501";
    }

    [Key]
    public int my_primary_key { get; set; }

    public string my_object_property { get; set; }
}