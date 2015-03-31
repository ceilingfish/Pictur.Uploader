using System;
using System.Net;
using Ceilingfish.Pictur.Core.Flickr.Api;

namespace Ceilingfish.Pictur.Core.Flickr
{
    public class ApiException : Exception
    {
        public Error Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ApiException(Error error)
            : base(error.Message)
        {
            Error = error;
        }

        public ApiException(string message, HttpStatusCode code)
            : base(message)
        {
            StatusCode = code;
        }
    }
}
