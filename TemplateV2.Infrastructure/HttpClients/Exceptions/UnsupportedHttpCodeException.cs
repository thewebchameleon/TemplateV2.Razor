using System;
using System.Net;

namespace TemplateV2.Infrastructure.HttpClients.Exceptions
{
    public class UnsupportedHttpCodeException : Exception
    {
        public HttpStatusCode Status { get; set; }

        public string Reason { get; set; }

        public UnsupportedHttpCodeException(HttpStatusCode status, string reason) : base($"An unhandled http status code was returned: '{status}', reason: '{reason}'")
        {
            Status = status;
            Reason = reason;
        }
    }

}
