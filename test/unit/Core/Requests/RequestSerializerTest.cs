using Microsoft.Extensions.Logging;
using Skarp.HubSpotClient.Dto;
using Skarp.HubSpotClient.Requests;
using Xunit;
using Xunit.Abstractions;

namespace Skarp.HubSpotClient.UnitTest
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
            const string expectedJson = "{\"properties\":[{\"property\":\"Email\",\"value\":\"testingapis@hubspot.com\"},{\"property\":\"FirstName\",\"value\":\"Adrian\"},{\"property\":\"Lastname\",\"value\":\"Mott\"},{\"property\":\"Website\",\"value\":\"http://hubspot.com\"},{\"property\":\"Company\",\"value\":\"HubSpot\"},{\"property\":\"Phone\",\"value\":\"555-122-2323\"},{\"property\":\"Address\",\"value\":\"25 First Street\"},{\"property\":\"City\",\"value\":\"Cambridge\"},{\"property\":\"State\",\"value\":\"MA\"},{\"property\":\"ZipCode\",\"value\":\"02139\"}]}";
            Assert.Equal(expectedJson, json);
        }
    }
}
