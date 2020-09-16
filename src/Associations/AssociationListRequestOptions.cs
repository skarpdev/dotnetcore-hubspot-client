using System;
using System.Collections.Generic;
using System.Text;

namespace Skarp.HubSpotClient.Associations
{
    public class AssociationListRequestOptions
    {
        private int _numberOfAssociationsToReturn = 100;

        /// <summary>
        /// Gets or sets the number of associations to return.
        /// </summary>
        /// <remarks>
        /// Defaults to 20 which is also the hubspot api default. Max value is 100
        /// </remarks>
        /// <value>
        /// The number of contacts to return.
        /// </value>
        public int NumberOfAssociationsToReturn
        {
            get => _numberOfAssociationsToReturn;
            set
            {
                if (value < 1 || value > 100)
                {
                    throw new ArgumentException(
                        $"Number of associations to return must be a positive ingeteger greater than 0 and less than 101 - you provided {value}");
                }
                _numberOfAssociationsToReturn = value;
            }
        }

        public long? AssociationOffset { get; set; } = null;

    }
}
