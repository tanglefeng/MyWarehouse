using Kengic.Was.Systems.Message;

namespace Kengic.Was.CrossCutting.Logging
{
    public static class FormatExtension
    {
        public static string FormatParameter(this string messageId, params string[] args)
        {
            var message = MessageRepository.GetMessage(messageId);
            var info = string.Join(",", args);
            var separator = message == messageId ? "," : ":";
            return message + separator + info;
        }
    }
}