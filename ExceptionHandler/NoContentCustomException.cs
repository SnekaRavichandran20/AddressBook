using System.Net;
using System.Runtime.Serialization;

namespace ExceptionHandler
{
    [Serializable]
    public sealed class NoContentCustomException : CustomException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private NoContentCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public NoContentCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.NoContent)
        {
        }
    }
}