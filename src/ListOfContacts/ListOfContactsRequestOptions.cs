using System;
using System.Collections.Generic;
using System.Text;

namespace Skarp.HubSpotClient.ListOfContacts
{
    public class ListOfContactsRequestOptions
    {
        private int _numberOfContactsToReturn = 100;

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
                if (value < 1 || value > 250)
                {
                    throw new ArgumentException(
                        $"Number of contacts to return must be a positive ingeteger greater than 0 and less than 251 - you provided {value}");
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
    }
}
