using System;

namespace hello.restaurant.api.APIs.Exceptions
{
    public class ClientNotSuccessException : Exception
    {
        private const string DEFAULT_MESAGE = "Http client status not success";
        public string Endpoint { get; }
        public int StatusCode { get; }
        public string Verb { get; }

        public ClientNotSuccessException()
           : base(DEFAULT_MESAGE)
        {
        }


        public ClientNotSuccessException(string message, Exception inner)
           : base(message, inner)
        {

        }

        public ClientNotSuccessException(int statuscode, string verb, string endpoint, string message)
            : base(message)
        {
            this.Verb = verb;
            this.StatusCode = statuscode;
            this.Endpoint = endpoint;
        }

    }


}