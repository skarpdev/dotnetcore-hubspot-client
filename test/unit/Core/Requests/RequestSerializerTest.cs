using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Skarp.HubSpotClient.Company;
using Skarp.HubSpotClient.Contact.Dto;
using Skarp.HubSpotClient.Core.Requests;
using Skarp.HubSpotClient.LineItem.Dto;
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
        public void RequestSerializer_serializes_non_hubspot_entities()
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

        [Fact]
        public void RequestSerializer_deserializes_list_of_entities()
        {
            var json = "[{'objectType':'LINE_ITEM','portalId':62515,'objectId':9845651,'properties':{'amount':{'value':'687.50','timestamp':0,'source':'CALCULATED','sourceId':'LineItemAmountCalculator'},'quantity':{'value':'25','timestamp':1525369755209,'source':'API','sourceId':null},'hs_lastmodifieddate':{'value':'0','timestamp':0,'source':'CALCULATED','sourceId':null},'price':{'value':'27.50','timestamp':1525299376761,'source':'API','sourceId':null},'name':{'value':'A custom name for the product for this line item.','timestamp':1525369755209,'source':'API','sourceId':null},'createdate':{'value':'0','timestamp':0,'source':'API','sourceId':null},'description':{'value':'A description of this product.','timestamp':1525299376761,'source':'API','sourceId':null},'hs_product_id':{'value':'1645342','timestamp':1525369755209,'source':'API','sourceId':null},'recurringbillingfrequency':{'value':'quarterly','timestamp':1525299376761,'source':'API','sourceId':null}},'version':0,'isDeleted':false},{'objectType':'LINE_ITEM','portalId':62515,'objectId':9867373,'properties':{'amount':{'value':'-275.00','timestamp':0,'source':'CALCULATED','sourceId':'LineItemAmountCalculator'},'quantity':{'value':'25','timestamp':1525369755209,'source':'API','sourceId':null},'hs_lastmodifieddate':{'value':'0','timestamp':0,'source':'CALCULATED','sourceId':null},'price':{'value':'9.00','timestamp':1525369755209,'source':'API','sourceId':null},'name':{'value':'Widgets, special discount price','timestamp':1525369755209,'source':'API','sourceId':null},'createdate':{'value':'0','timestamp':0,'source':'API','sourceId':null},'discount':{'value':'20','timestamp':1525292253568,'source':'API','sourceId':null},'description':{'value':'A description of yet another product.','timestamp':1525289943771,'source':'API','sourceId':null},'hs_product_id':{'value':'1645187','timestamp':1525369755209,'source':'API','sourceId':null},'recurringbillingfrequency':{'value':'annually','timestamp':1525289943771,'source':'API','sourceId':null}},'version':0,'isDeleted':false}]";
            var result = _serializer.DeserializeEntities<LineItemHubSpotEntity>(json);

            Assert.Equal(2, result.Count());
            Assert.DoesNotContain(result, x => string.IsNullOrEmpty(x.Name));
        }

        [Fact]
        public void RequestSerializer_serializes_entity_to_namevaluelist()
        {
            var json = _serializer.SerializeEntityToNameValueList(_contactDto);
            const string expectedJson = "[{\"value\":\"testingapis@hubspot.com\",\"name\":\"email\"},{\"value\":\"Adrian\",\"name\":\"firstname\"},{\"value\":\"Mott\",\"name\":\"lastname\"},{\"value\":\"http://hubspot.com\",\"name\":\"website\"},{\"value\":\"HubSpot\",\"name\":\"company\"},{\"value\":\"555-122-2323\",\"name\":\"phone\"},{\"value\":\"25 First Street\",\"name\":\"address\"},{\"value\":\"Cambridge\",\"name\":\"city\"},{\"value\":\"MA\",\"name\":\"state\"},{\"value\":\"02139\",\"name\":\"zip\"}]";
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void RequestSerializer_serializes_list_of_entities_to_namevaluelist()
        {
            var listOfEntities = new[]{ _contactDto };

            var json = _serializer.SerializeEntitiesToNameValueList(listOfEntities);

            const string expectedJson = "[[{\"value\":\"testingapis@hubspot.com\",\"name\":\"email\"},{\"value\":\"Adrian\",\"name\":\"firstname\"},{\"value\":\"Mott\",\"name\":\"lastname\"},{\"value\":\"http://hubspot.com\",\"name\":\"website\"},{\"value\":\"HubSpot\",\"name\":\"company\"},{\"value\":\"555-122-2323\",\"name\":\"phone\"},{\"value\":\"25 First Street\",\"name\":\"address\"},{\"value\":\"Cambridge\",\"name\":\"city\"},{\"value\":\"MA\",\"name\":\"state\"},{\"value\":\"02139\",\"name\":\"zip\"}]]";
            Assert.Equal(expectedJson, json);
        }
    }
}
