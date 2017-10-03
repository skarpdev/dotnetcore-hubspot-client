using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Skarp.HubSpotClient.UnitTest.Core
{
    public class HubspotExceptionTest
    {
        [Fact]
        public void HubspotException_exception_with_nojson_nomessage_renders_correct_message()
        {
            try
            {
                throw new HubSpotException();
            }
            catch (HubSpotException ex)
            {
                Assert.Equal("Exception of type 'Skarp.HubSpotClient.HubSpotException' was thrown., JSONResponse=Empty", ex.Message);
            }
        }

        [Fact]
        public void HubspotException_exception_with_nojson_message_renders_correct_message()
        {
            try
            {
                throw new HubSpotException("Test Message");
            }
            catch (HubSpotException ex)
            {
                Assert.Equal("Test Message, JSONResponse=Empty", ex.Message);
            }
        }

        [Fact]
        public void HubspotException_exception_with_json_message_renders_correct_message()
        {
            try
            {
                throw new HubSpotException("Test Message" , "JSON Message");
            }
            catch (HubSpotException ex)
            {
                Assert.Equal("Test Message, JSONResponse=JSON Message", ex.Message);
            }
        }
    }
}
