using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        [Fact]
        public void RequestDataConvert_converts_response_to_entity()
        {
            var json = "{'identity-profiles':[{'identities': [{'timestamp': 1331075050646,'type':'EMAIL','value':'fumanchu@hubspot.com'},{'timestamp': 1331075050681,'type': 'LEAD_GUID','value': '22a26060-c9d7-44b0-9f07-aa40488cfa3a'      }    ],    'vid': 61574  }],'properties': {  'website': {    'value': 'http://hubspot.com',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'http: //hubspot.com',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'city': {    'value': 'Cambridge',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'Cambridge',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'firstname': {    'value': 'Adrian',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'Adrian',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'zipcode': {    'value': '02139',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': '02139',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'lastname': {    'value': 'Mott',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'Mott',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'company': {    'value': 'HubSpot',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'HubSpot',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'phone': {    'value': '555-122-2323',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': '555-122-2323',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'state': {    'value': 'MA',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'MA',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'address': {    'value': '25FirstStreet',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': '25FirstStreet',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'email': {    'value': 'fumanchu@hubspot.com',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'fumanchu@hubspot.com',        'source-type': 'API',        'source-id':\"None\"}]}},'form-submissions': [],'vid': 61574}";
            var jobj = JsonConvert.DeserializeObject<ExpandoObject>(json);

            var contact = _converter.FromHubSpotResponse<ContactHubSpotEntity>(jobj);
            Assert.NotNull(contact);

            Assert.Equal(contact.Vid, 61574);
            Assert.Equal(contact.Website, "http://hubspot.com");
            Assert.Equal(contact.Email, "fumanchu@hubspot.com");
            Assert.Equal(contact.FirstName, "Adrian");
            Assert.Equal(contact.ZipCode, "02139");
        }

        private class CustomContactHubSpotEntity : ContactHubSpotEntity
        {
            public string MyCustomProp { get; set; }
        }
    }
}
