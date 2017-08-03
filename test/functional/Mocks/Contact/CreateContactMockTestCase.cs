using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RapidCore.Network;
using Skarp.HubSpotClient.Core;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks.Contact
{
    public class CreateContactMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.EndsWith("/contacts/v1/contact");
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse =
                "{'identity-profiles':[{'identities': [{'timestamp': 1331075050646,'type':'EMAIL','value':'fumanchu@hubspot.com'},{'timestamp': 1331075050681,'type': 'LEAD_GUID','value': '22a26060-c9d7-44b0-9f07-aa40488cfa3a'      }    ],    'vid': 61574  }],'properties': {  'website': {    'value': 'http://hubspot.com',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'http: //hubspot.com',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'city': {    'value': 'Cambridge',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'Cambridge',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'firstname': {    'value': 'Adrian',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'Adrian',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'zip': {    'value': '02139',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': '02139',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'lastname': {    'value': 'Mott',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'Mott',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'company': {    'value': 'HubSpot',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'HubSpot',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'phone': {    'value': '555-122-2323',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': '555-122-2323',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'state': {    'value': 'MA',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'MA',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'address': {    'value': '25FirstStreet',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': '25FirstStreet',        'source-type': 'API',        'source-id':\"None\"      }    ]  },  'email': {    'value': 'fumanchu@hubspot.com',    'versions': [      {        'timestamp': 1331075050646,        'selected':false,        'source-label':\"None\",        'value': 'fumanchu@hubspot.com',        'source-type': 'API',        'source-id':\"None\"}]}},'form-submissions': [],'vid': 61574}";
            
            response.Content = new JsonContent(jsonResponse);
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}