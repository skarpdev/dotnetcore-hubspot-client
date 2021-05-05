using Skarp.HubSpotClient.Core.Interfaces;
using System.Collections.Generic;

namespace Skarp.HubSpotClient.ListOfContacts.Interfaces
{
    public interface ICreateContactListRequestHubSpotEntity : IHubSpotEntity
    {
        bool? Dynamic { get; set; }
        List<List<IContactListFilter>> Filters { get; set; }
        bool HasMore { get; set; }
        string Name { get; set; }
        int offset { get; set; }
        int? PortalId { get; set; }

    }
}