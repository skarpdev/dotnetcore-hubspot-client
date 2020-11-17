using System;
using System.Collections.Generic;

namespace Skarp.HubSpotClient.Contact
{
    public enum ContactFormSubmissionMode
    {
        All,
        None,
        Newest,
        Oldest
    }

    public class ContactGetRequestOptions
    {
        public List<string> PropertiesToInclude { get; set; } = new List<string>();
        public bool IncludeHistory { get; set; } = true; // this is the default in HubSpot
        public ContactFormSubmissionMode FormSubmissionMode { get; set; } = ContactFormSubmissionMode.All; // this is the default in HubSpot
        public bool IncludeListMemberships { get; set; } = true; // this is the default in HubSpot
    }

    public class ContactListRequestOptions
    {
        private int _numberOfContactsToReturn = 20;

        /// <summary>
        /// Gets or sets the number of contacts to return.
        /// </summary>
        /// <remarks>
        /// Defaults to 20 which is also the hubspot api default. Max value is 100
        /// </remarks>
        /// <value>
        /// The number of contacts to return.
        /// </value>
        public int NumberOfContactsToReturn
        {
            get => _numberOfContactsToReturn;
            set
            {
                if (value < 1 || value > 100)
                {
                    throw new ArgumentException(
                        $"Number of contacts to return must be a positive ingeteger greater than 0 - you provided {value}");
                }
                _numberOfContactsToReturn = value;
            }
        }

        /// <summary>
        /// Get or set the continuation offset when calling list many times to enumerate all your contacts
        /// </summary>
        /// <remarks>
        /// The return DTO from List contains the current "offset" that you can inject into your next list call 
        /// to continue the listing process
        /// </remarks>
        public long? ContactOffset { get; set; } = null;
        public List<string> PropertiesToInclude { get; set; } = new List<string>();
    }
}
