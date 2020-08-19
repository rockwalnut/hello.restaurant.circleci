using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using hello.restaurant.api.Models;

namespace hello.restaurant.api.Exceptions
{
    public class UserNotAuthorizeException : Exception
    {
        private const string DEFAULT_MESSAGE = "User not authorize";
        public string rev { get; }
        public string value { get; }

        public UserNotAuthorizeException()
           : base(DEFAULT_MESSAGE)
        {
        }

        public UserNotAuthorizeException(string message)
            : base(message)
        {
        }

        public UserNotAuthorizeException(string message, Exception inner)
       : base(message, inner)
        {
        }
    }

    public class UserNotAuthenException : Exception
    {
        private const string DEFAULT_MESSAGE = "User not authenthication";
        public string rev { get; }
        public string value { get; }

        public UserNotAuthenException()
           : base(DEFAULT_MESSAGE)
        {
        }

        public UserNotAuthenException(string message)
            : base(message)
        {
        }

        public UserNotAuthenException(string message, Exception inner)
       : base(message, inner)
        {
        }
    }

}