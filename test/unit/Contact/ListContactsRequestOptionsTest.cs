using System;
using Skarp.HubSpotClient.Contact;
using Xunit;

namespace Skarp.HubSpotClient.UnitTest.Contact
{
    public class ListContactsRequestOptionsTest
    {
        private ContactListRequestOptions _opts;

        public ListContactsRequestOptionsTest()
        {
            _opts = new ContactListRequestOptions();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-9999)]
        [InlineData(int.MinValue)]
        [InlineData(101)]
        [InlineData(int.MaxValue)]
        public void ListContactRequestOptions_set_contacts_to_return_validates(int theValue)
        {
            var ex = Record.Exception(() =>_opts.NumberOfContactsToReturn = theValue);
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void ListContactRequestOptions_set_contacts_with_valid_value_does_not_throw()
        {
            // NO EXECPTION SHALL BE THROWN
            const int daValue = 50;
            _opts.NumberOfContactsToReturn = daValue;
            Assert.Equal(daValue, _opts.NumberOfContactsToReturn);
        }
    }
}
