namespace Skarp.HubSpotClient.ListOfContacts.Interfaces
{
    public interface IContactListFilter
    {
        string op { get; set; }
        string Property { get; set; }
        string Type { get; set; }
        string Value { get; set; }
    }
}