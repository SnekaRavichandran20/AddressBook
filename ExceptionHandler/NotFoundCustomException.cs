using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{
    [Serializable]
    public sealed class NotFoundCustomException : CustomException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private NotFoundCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public NotFoundCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.NotFound)
        {
        }
    }
}