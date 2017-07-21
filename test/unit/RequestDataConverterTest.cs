using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skarp.HubSpotClient.Dto;
using Skarp.HubSpotClient.Requests;
using Xunit;

namespace Skarp.HubSpotClient.UnitTest
{
    public class RequestDataConverterTest
    {
        private readonly RequestDataConverter _converter;
        private readonly ContactHubSpotEntity _contactDto;
        private CustomContactHubSpotEntity _customContactDto;

        public RequestDataConverterTest()
        {
            _converter = new RequestDataConverter();
            _contactDto = new ContactHubSpotEntity()
            {
                Address = "25 First Street",
                City = "Cambridge",
                Company = "HubSpot",
                Email = "testingapis@hubspot.com",
                FirstName = "Adrian",
                Lastname = "Mott",
                Phone = "555-122-2323",
                State = "MA",
                Website = "http://hubspot.com",
                ZipCode = "02139"
            };
            
            _customContactDto = new CustomContactHubSpotEntity
            {
                Address = "25 First Street",
                City = "Cambridge",
                Company = "HubSpot",
                Email = "testingapis@hubspot.com",
                FirstName = "Adrian",
                Lastname = "Mott",
                Phone = "555-122-2323",
                State = "MA",
                Website = "http://hubspot.com",
                ZipCode = "02139",
                MyCustomProp = "Has a value!"
            };
        }


        [Fact]
        public void RequestDataConverter_converts_contact_to_internal_representation()
        {
            var converted = _converter.ToHubspotDataEntity(_contactDto);

            Assert.NotNull(converted);
            Assert.NotEmpty(converted.Properties);

            Assert.True(converted.Properties.Count == 10, $"converted.Properties.Count == 10, count is {converted.Properties.Count}");

            foreach (var prop in converted.Properties)
            {
                Assert.NotNull(prop.Property);
                Assert.NotNull(prop.Value);
            }
        }

        [Fact]
        public void RequestDataConverter_converts_contact_custom_to_internal_representation()
        {
            var converted = _converter.ToHubspotDataEntity(_customContactDto);

            Assert.NotNull(converted);
            Assert.NotEmpty(converted.Properties);

            Assert.True(converted.Properties.Count == 11, $"converted.Properties.Count == 11, count is {converted.Properties.Count}");
            
            // check that the custom prop was properly mapped
            var prop = converted.Properties.Single(q => q.Property == "MyCustomProp");
            Assert.Equal(prop.Value, "Has a value!");
        }


        private class CustomContactHubSpotEntity : ContactHubSpotEntity
        {
            public string MyCustomProp { get; set; }
        }
    }
}
