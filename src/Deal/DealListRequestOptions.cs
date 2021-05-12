using System;
using System.Collections.Generic;

namespace Skarp.HubSpotClient.Deal
{
    public class DealListRequestOptions
    {
        private int _numberOfDealsToReturn = 100;

        /// <summary>
        /// Gets or sets the number of deals to return.
        /// </summary>
        /// <remarks>
        /// Defaults to 20 which is also the hubspot api default. Max value is 100
        /// </remarks>
        /// <value>
        /// The number of deals to return.
        /// </value>
        public int NumberOfDealsToReturn
        {
            get => _numberOfDealsToReturn;
            set
            {
                if (value < 1 || value > 250)
                {
                    throw new ArgumentException(
                        $"Number of deals to return must be a positive integer greater than 0 and less than 251 - you provided {value}");
                }
                _numberOfDealsToReturn = value;
            }
        }

        /// <summary>
        /// Get or set the continuation offset when calling list many times to enumerate all your deals
        /// </summary>
        /// <remarks>
        /// The return DTO from List contains the current "offset" that you can inject into your next list call 
        /// to continue the listing process
        /// </remarks>
        public long? DealOffset { get; set; } = null;

        public List<string> PropertiesToInclude { get; set; } = new List<string>();

    }
}
