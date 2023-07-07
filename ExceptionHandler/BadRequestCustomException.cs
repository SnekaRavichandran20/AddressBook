using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{
    [Serializable]
    public sealed class BadRequestCustomException : CustomException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private BadRequestCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public BadRequestCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.BadRequest)
        {
        }
    }
}