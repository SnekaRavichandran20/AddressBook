using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{
    [Serializable]
    public sealed class InternalServerCustomException : CustomException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private InternalServerCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public InternalServerCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.InternalServerError)
        {
        }
    }
}