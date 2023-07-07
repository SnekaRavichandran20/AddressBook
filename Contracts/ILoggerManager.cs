
namespace Contracts
{
    public interface ILoggerManager
    {
        /// <summary>
        /// 
        /// </summary>
        void LogInfo(string message);
        /// <summary>
        /// 
        /// </summary>
        void LogWarn(string message);
        /// <summary>
        /// 
        /// </summary>
        void LogDebug(string message);
        /// <summary>
        /// 
        /// </summary>
        void LogError(string message);
    }
}