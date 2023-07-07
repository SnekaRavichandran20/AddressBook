using System;
using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{

    [Serializable]
    public class CustomException : Exception
    {
        public int Code { get; }

        public string Description { get; }

        public CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CustomException(string message, string description, int code) : base(message)
        {
            Code = code;
            Description = description;
        }
    }
}