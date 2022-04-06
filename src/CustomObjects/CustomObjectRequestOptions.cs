using System;
using System.Collections.Generic;

namespace Skarp.HubSpotClient.CustomObjects
{
    public class CustomObjectRequestOptions
    {
        private int _numberOfItemsToReturn = 20;

        /// <summary>
        /// Gets or sets the number of custom objects to return.
        /// </summary>
        /// <remarks>
        /// Defaults to 20 which is also the hubspot api default. Max value is 100
        /// </remarks>
        /// <value>
        /// The number of custom objects to return.
        /// </value>
        public int NumberOfCustomObjectsToReturn
        {
            get => _numberOfItemsToReturn;
            set
            {
                if (value < 1 || value > 100)
                {
                    throw new ArgumentException(
                        $"Number of custom objects to return must be a positive integer greater than 0 - you provided {value}");
                }
                _numberOfItemsToReturn = value;
            }
        }

        /// <summary>
        /// Get or set the continuation offset when calling list many times to enumerate all your items
        /// </summary>
        /// <remarks>
        /// The return DTO from List of objects the current "offset" that you can inject into your next list call 
        /// to continue the listing process
        /// </remarks>
        public long? ItemsOffset { get; set; } = null;
        public List<string> PropertiesToInclude { get; set; } = new List<string>();
        public bool UseCustomKeyProperty { get; set; }
    }
}
