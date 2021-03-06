using System;
using System.Net;

namespace LibraryAPI.Errors
{
    public class RestException : Exception
    {
        public HttpStatusCode Code { get; set; }
        public object Errors { get; }

        public RestException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }
    }
}