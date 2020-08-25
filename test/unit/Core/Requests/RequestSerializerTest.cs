using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Skarp.HubSpotClient.Company;
using Skarp.HubSpotClient.Contact.Dto;
using Skarp.HubSpotClient.Core.Requests;
using Xunit;
using Xunit.Abstractions;

namespace Skarp.HubSpotClient.UnitTest.Core.Requests
{
    public class RequestSerializerTest : UnitTestBase<RequestSerializer>
    {
        private readonly ITestOutputHelper _output;
        private readonly RequestSerializer _serializer;
        private readonly ContactHubSpotEntity _contactDto;
        private readonly CompanySearchByDomain _companySearch;

        public RequestSerializerTest(ITestOutputHelper output) : base(output)
        {
            _output = output;
            _serializer =
                new RequestSerializer(new RequestDataConverter(LoggerFactory.CreateLogger<RequestDataConverter>()));
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

            _companySearch = new CompanySearchByDomain
            {
                Limit = 2,
                Offset = new CompanySearchOffset
                {
                    CompanyId = 0,
                    IsPrimary = true
                },
                RequestOptions = new CompanySearchRequestOptions
                {
                    Properties = new List<string>
                    {
                        "domain",
                        "createdate",
                        "name",
                        "hs_lastmodifieddate"
                    }
                }
            };
        }
        
        [Fact]
        public void RequestSerializer_serializes_contact_entity()
        {
            var json = _serializer.SerializeEntity(_contactDto);
            const string expectedJson = "{\"properties\":[{\"property\":\"email\",\"value\":\"testingapis@hubspot.com\"},{\"property\":\"firstname\",\"value\":\"Adrian\"},{\"property\":\"lastname\",\"value\":\"Mott\"},{\"property\":\"website\",\"value\":\"http://hubspot.com\"},{\"property\":\"company\",\"value\":\"HubSpot\"},{\"property\":\"phone\",\"value\":\"555-122-2323\"},{\"property\":\"address\",\"value\":\"25 First Street\"},{\"property\":\"city\",\"value\":\"Cambridge\"},{\"property\":\"state\",\"value\":\"MA\"},{\"property\":\"zip\",\"value\":\"02139\"}]}";
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void RequestSerializer_serialzies_non_hubspot_entities()
        {
            var json = _serializer.SerializeEntity(_companySearch);
            const string expectedJson =
                "{\"limit\":2,\"requestOptions\":{\"properties\":[\"domain\",\"createdate\",\"name\",\"hs_lastmodifieddate\"]},\"offset\":{\"isPrimary\":true,\"companyId\":0}}";
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void RequestSerializer_serializes_contact_entities()
        {
            var json = _serializer.SerializeEntities(new List<ContactHubSpotEntity>{ _contactDto, _contactDto });
            const string expectedJson = "[{\"properties\":[{\"property\":\"email\",\"value\":\"testingapis@hubspot.com\"},{\"property\":\"firstname\",\"value\":\"Adrian\"},{\"property\":\"lastname\",\"value\":\"Mott\"},{\"property\":\"website\",\"value\":\"http://hubspot.com\"},{\"property\":\"company\",\"value\":\"HubSpot\"},{\"property\":\"phone\",\"value\":\"555-122-2323\"},{\"property\":\"address\",\"value\":\"25 First Street\"},{\"property\":\"city\",\"value\":\"Cambridge\"},{\"property\":\"state\",\"value\":\"MA\"},{\"property\":\"zip\",\"value\":\"02139\"}]},{\"properties\":[{\"property\":\"email\",\"value\":\"testingapis@hubspot.com\"},{\"property\":\"firstname\",\"value\":\"Adrian\"},{\"property\":\"lastname\",\"value\":\"Mott\"},{\"property\":\"website\",\"value\":\"http://hubspot.com\"},{\"property\":\"company\",\"value\":\"HubSpot\"},{\"property\":\"phone\",\"value\":\"555-122-2323\"},{\"property\":\"address\",\"value\":\"25 First Street\"},{\"property\":\"city\",\"value\":\"Cambridge\"},{\"property\":\"state\",\"value\":\"MA\"},{\"property\":\"zip\",\"value\":\"02139\"}]}]";
            Assert.Equal(expectedJson, json);
        }
    }
}
