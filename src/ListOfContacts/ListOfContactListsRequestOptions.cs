using System;

namespace Skarp.HubSpotClient.ListOfContacts
{
    public class ListOfContactListsRequestOptions
    {
        private int _numberOfContactListsToReturn = 100;

        /// <summary>
        /// Gets or sets the number of contact lists to return.
        /// </summary>
        /// <remarks>
        /// Defaults to 20 which is also the hubspot api default. Max value is 100
        /// </remarks>
        /// <value>
        /// The number of contacts to return.
        /// </value>
        public int NumberOfContactListsToReturn
        {
            get => _numberOfContactListsToReturn;
            set
            {
                if (value < 1 || value > 250)
                {
                    throw new ArgumentException(
                        $"Number of contacts to return must be a positive ingeteger greater than 0 and less than 251 - you provided {value}");
                }
                _numberOfContactListsToReturn = value;
            }
        }

        /// <summary>
        /// Get or set the continuation offset when calling list many times to enumerate all your contact lists
        /// </summary>
        /// <remarks>
        /// The return DTO from List contains the current "offset" that you can inject into your next list call 
        /// to continue the listing process
        /// </remarks>
        public long? ContactListOffset { get; set; } = null;
    }
}