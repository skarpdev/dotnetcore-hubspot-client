using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RapidCore.Network;

namespace Skarp.HubSpotClient.FunctionalTests.Mocks
{
    public class ListContactMockTestCase : IMockRapidHttpClientTestCase
    {
        public bool IsMatch(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.Contains("/contacts/v1/lists/all/contacts/all");
        }

        public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            const string jsonResponse =
               "{\"contacts\":[{\"addedAt\":1390574181854,\"vid\":204727,\"canonical-vid\":204727,\"merged-vids\":[],\"portal-id\":62515,\"is-contact\":true,\"profile-token\":\"AO_T-mMusl38dq-ff-Lms9BvB5nWgFb7sFrDU98e-3CBdnB7G2qCt1pMEHC9zmqSfOkeq2on6Dz72P-iLoGjEXfLuWfvZRWBpkB-C9Enw6SZ-ZASg57snQun5f32ISDfLOiK7BYDL0l2\",\"profile-url\":\"https://app.hubspot.com/contacts/62515/lists/public/contact/_AO_T-mMusl38dq-ff-Lms9BvB5nWgFb7sFrDU98e-3CBdnB7G2qCt1pMEHC9zmqSfOkeq2on6Dz72P-iLoGjEXfLuWfvZRWBpkB-C9Enw6SZ-ZASg57snQun5f32ISDfLOiK7BYDL0l2/\",\"properties\":{\"firstname\":{\"value\":\"Bob\"},\"lastmodifieddate\":{\"value\":\"1483461406481\"},\"company\":{\"value\":\"\"},\"lastname\":{\"value\":\"Record\"}},\"form-submissions\":[],\"identity-profiles\":[{\"vid\":204727,\"saved-at-timestamp\":1476768116149,\"deleted-changed-timestamp\":0,\"identities\":[{\"type\":\"LEAD_GUID\",\"value\":\"f9d728f1-dff1-49b0-9caa-247dbdf5b8b7\",\"timestamp\":1390574181878},{\"type\":\"EMAIL\",\"value\":\"mgnew-email@hubspot.com\",\"timestamp\":1476768116137}]}],\"merge-audits\":[]},{\"addedAt\":1392643921079,\"vid\":207303,\"canonical-vid\":207303,\"merged-vids\":[],\"portal-id\":62515,\"is-contact\":true,\"profile-token\":\"AO_T-mPMwvuZG_QTNH28c_MbhSyNRuuTNw9I7zJAaMFjOqL9HKlH9uBteqHAiTRUWVAPTThuU-Fmy7IemUNUvdtYpLrsll6nw47qnu7ACiSHFR6qZP1tDVZFpxueESKiKUIIvRjGzt8P\",\"profile-url\":\"https://app.hubspot.com/contacts/62515/lists/public/contact/_AO_T-mPMwvuZG_QTNH28c_MbhSyNRuuTNw9I7zJAaMFjOqL9HKlH9uBteqHAiTRUWVAPTThuU-Fmy7IemUNUvdtYpLrsll6nw47qnu7ACiSHFR6qZP1tDVZFpxueESKiKUIIvRjGzt8P/\",\"properties\":{\"firstname\":{\"value\":\"Ff_FirstName_0\"},\"lastmodifieddate\":{\"value\":\"1479148429488\"},\"lastname\":{\"value\":\"Ff_LastName_0\"}},\"form-submissions\":[],\"identity-profiles\":[{\"vid\":207303,\"saved-at-timestamp\":1392643921090,\"deleted-changed-timestamp\":0,\"identities\":[{\"type\":\"EMAIL\",\"value\":\"email_0be34aebe5@abctest.com\",\"timestamp\":1392643921079},{\"type\":\"LEAD_GUID\",\"value\":\"058378c6-9513-43e1-a13a-43a98d47aa22\",\"timestamp\":1392643921082}]}],\"merge-audits\":[]}],\"has-more\":true,\"vid-offset\":207303}";


            response.Content = new JsonContent(jsonResponse);
            response.RequestMessage = request;

            return Task.FromResult(response);
        }
    }
}