using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Skarp.HubSpotClient.Dto;
using Skarp.HubSpotClient.Requests;
using Xunit;
using Xunit.Abstractions;

namespace Skarp.HubSpotClient.UnitTest
{
    public class RequestDataConverterTest : UnitTestBase<RequestDataConverter>
    {
        private readonly RequestDataConverter _converter;
        private readonly ContactHubSpotEntity _contactDto;
        private readonly CustomContactHubSpotEntity _customContactDto;
        private CompanyHubSpotEntity _companyDto;

        public RequestDataConverterTest(ITestOutputHelper output) : base(output)
        {
            _converter = new RequestDataConverter(Logger);
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

            _companyDto = new CompanyHubSpotEntity
            {
                Name = "Acme Inc",
                Description = "The world famous Acme Inc"
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

        [Fact]
        public void RequestDataConvert_converts_company_to_internal_repsentation()
        {
            var converted = _converter.ToHubspotDataEntity(_companyDto);
            
            Assert.NotNull(converted);
            var allProps = converted.Properties;
            Assert.NotEmpty(allProps);
            
            Assert.True(allProps.Count == 2, "converted.Properties.Count == 2");

            foreach (var prop in allProps)
            {
                Assert.NotNull(prop.Name);
                Assert.NotNull(prop.Value);
            }
        }

        private class CustomContactHubSpotEntity : ContactHubSpotEntity
        {
            public string MyCustomProp { get; set; }
        }
    }
}
