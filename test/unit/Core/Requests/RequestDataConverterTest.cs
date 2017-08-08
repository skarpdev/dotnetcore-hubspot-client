using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Skarp.HubSpotClient.Company.Dto;
using Skarp.HubSpotClient.Contact.Dto;
using Skarp.HubSpotClient.Core.Requests;
using Xunit;
using Xunit.Abstractions;

namespace Skarp.HubSpotClient.UnitTest.Core.Requests
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
                MyCustomProp = "Has a value!",
                IgnoreMe = "Even though I have a value! muhahahah"
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

            Assert.Equal(61574, contact.Id);
            Assert.Equal("http://hubspot.com", contact.Website);
            Assert.Equal("fumanchu@hubspot.com", contact.Email);
            Assert.Equal("Adrian", contact.FirstName);
            Assert.Equal("02139", contact.ZipCode);
        }

        [Fact]
        public void RequestDataConvert_converts_list_response_to_entities()
        {
            var json =
                "{\"contacts\":[{\"addedAt\":1390574181854,\"vid\":204727,\"canonical-vid\":204727,\"merged-vids\":[],\"portal-id\":62515,\"is-contact\":true,\"profile-token\":\"AO_T-mMusl38dq-ff-Lms9BvB5nWgFb7sFrDU98e-3CBdnB7G2qCt1pMEHC9zmqSfOkeq2on6Dz72P-iLoGjEXfLuWfvZRWBpkB-C9Enw6SZ-ZASg57snQun5f32ISDfLOiK7BYDL0l2\",\"profile-url\":\"https://app.hubspot.com/contacts/62515/lists/public/contact/_AO_T-mMusl38dq-ff-Lms9BvB5nWgFb7sFrDU98e-3CBdnB7G2qCt1pMEHC9zmqSfOkeq2on6Dz72P-iLoGjEXfLuWfvZRWBpkB-C9Enw6SZ-ZASg57snQun5f32ISDfLOiK7BYDL0l2/\",\"properties\":{\"firstname\":{\"value\":\"Bob\"},\"lastmodifieddate\":{\"value\":\"1483461406481\"},\"company\":{\"value\":\"\"},\"lastname\":{\"value\":\"Record\"}},\"form-submissions\":[],\"identity-profiles\":[{\"vid\":204727,\"saved-at-timestamp\":1476768116149,\"deleted-changed-timestamp\":0,\"identities\":[{\"type\":\"LEAD_GUID\",\"value\":\"f9d728f1-dff1-49b0-9caa-247dbdf5b8b7\",\"timestamp\":1390574181878},{\"type\":\"EMAIL\",\"value\":\"mgnew-email@hubspot.com\",\"timestamp\":1476768116137}]}],\"merge-audits\":[]},{\"addedAt\":1392643921079,\"vid\":207303,\"canonical-vid\":207303,\"merged-vids\":[],\"portal-id\":62515,\"is-contact\":true,\"profile-token\":\"AO_T-mPMwvuZG_QTNH28c_MbhSyNRuuTNw9I7zJAaMFjOqL9HKlH9uBteqHAiTRUWVAPTThuU-Fmy7IemUNUvdtYpLrsll6nw47qnu7ACiSHFR6qZP1tDVZFpxueESKiKUIIvRjGzt8P\",\"profile-url\":\"https://app.hubspot.com/contacts/62515/lists/public/contact/_AO_T-mPMwvuZG_QTNH28c_MbhSyNRuuTNw9I7zJAaMFjOqL9HKlH9uBteqHAiTRUWVAPTThuU-Fmy7IemUNUvdtYpLrsll6nw47qnu7ACiSHFR6qZP1tDVZFpxueESKiKUIIvRjGzt8P/\",\"properties\":{\"firstname\":{\"value\":\"Ff_FirstName_0\"},\"lastmodifieddate\":{\"value\":\"1479148429488\"},\"lastname\":{\"value\":\"Ff_LastName_0\"}},\"form-submissions\":[],\"identity-profiles\":[{\"vid\":207303,\"saved-at-timestamp\":1392643921090,\"deleted-changed-timestamp\":0,\"identities\":[{\"type\":\"EMAIL\",\"value\":\"email_0be34aebe5@abctest.com\",\"timestamp\":1392643921079},{\"type\":\"LEAD_GUID\",\"value\":\"058378c6-9513-43e1-a13a-43a98d47aa22\",\"timestamp\":1392643921082}]}],\"merge-audits\":[]}],\"has-more\":true,\"vid-offset\":207303}";

            var jobj = JsonConvert.DeserializeObject<ExpandoObject>(json);

            var contactList = _converter.FromHubSpotListResponse<ContactListHubSpotEntity<ContactHubSpotEntity>>(jobj);

            Assert.NotNull(contactList);
            Assert.True(contactList.MoreResultsAvailable);
            Assert.InRange(contactList.ContinuationOffset, 1, long.MaxValue);
            Assert.True(2 == contactList.Contacts.Count, $"2 == contactList.Contacts.Count - have count {contactList.Contacts.Count}");

            var bob = contactList.Contacts.First();
            Assert.Equal("Bob", bob.FirstName);
            Assert.Equal("Record", bob.Lastname);

            var other = contactList.Contacts.Last();
            Assert.Equal("Ff_FirstName_0", other.FirstName);
            Assert.Equal("Ff_LastName_0", other.Lastname);
        }

        private class CustomContactHubSpotEntity : ContactHubSpotEntity
        {
            public string MyCustomProp { get; set; }

            [IgnoreDataMember]
            public string IgnoreMe { get; set; }
        }
    }
}
