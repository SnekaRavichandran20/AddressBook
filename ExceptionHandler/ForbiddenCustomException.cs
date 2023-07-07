using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{
    [Serializable]
    public sealed class ForbiddenCustomException : CustomException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private ForbiddenCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public ForbiddenCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.Forbidden)
        {
        }
    }
}