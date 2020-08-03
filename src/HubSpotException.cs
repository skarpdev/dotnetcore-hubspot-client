using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Skarp.HubSpotClient
{
    [Serializable]
    public class HubSpotException : Exception
    {
        public HttpResponseMessage Response { get; }

        public string RawJsonResponse { get;  }
        public HubSpotException()
        {
        }

        public HubSpotException(string message) : base(message)
        {
        }

        public HubSpotException(string message, string jsonResponse) : base(message)
        {
            RawJsonResponse = jsonResponse;
        }

        public HubSpotException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public HubSpotException(string message, string jsonResponse, HttpResponseMessage response) : this(message, jsonResponse)
        {
            this.Response = response;
        }

        public override String Message
        {
            get
            {
                return base.Message + $", JSONResponse={RawJsonResponse??"Empty"}";
            }
        }
    }
}
