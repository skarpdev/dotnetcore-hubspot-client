using System.Collections.Generic;

namespace Skarp.HubSpotClient.Contact
{
    public class ContactGetRequestOptions
    {
        public List<string> PropertiesToInclude { get; set; } = new List<string>();
        public bool IncludeHistory { get; set; } = true; // this is the default in HubSpot
        public ContactFormSubmissionMode FormSubmissionMode { get; set; } = ContactFormSubmissionMode.All; // this is the default in HubSpot
        public bool IncludeListMemberships { get; set; } = true; // this is the default in HubSpot
    }
}