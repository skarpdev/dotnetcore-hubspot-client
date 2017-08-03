using System.Net.Http;
using System.Text;

namespace Skarp.HubSpotClient
{
    public class JsonContent : StringContent
    {
        public JsonContent(string json) : this(json, Encoding.UTF8)
        {
        }

        public JsonContent(string json, Encoding encoding) : base(json, encoding, "application/json")
        {
        }
    }
}