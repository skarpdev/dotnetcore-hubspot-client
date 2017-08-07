using Microsoft.Extensions.Logging;
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
        }
        
        [Fact]
        public void RequestSerializer_serializes_contact_entity()
        {
            var json = _serializer.SerializeEntity(_contactDto);
            const string expectedJson = "{\"properties\":[{\"property\":\"email\",\"value\":\"testingapis@hubspot.com\"},{\"property\":\"firstname\",\"value\":\"Adrian\"},{\"property\":\"lastname\",\"value\":\"Mott\"},{\"property\":\"website\",\"value\":\"http://hubspot.com\"},{\"property\":\"company\",\"value\":\"HubSpot\"},{\"property\":\"phone\",\"value\":\"555-122-2323\"},{\"property\":\"address\",\"value\":\"25 First Street\"},{\"property\":\"city\",\"value\":\"Cambridge\"},{\"property\":\"state\",\"value\":\"MA\"},{\"property\":\"zipcode\",\"value\":\"02139\"}]}";
            Assert.Equal(expectedJson, json);
        }
    }
}
