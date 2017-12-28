using System.Collections.Concurrent;
using System.Linq;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.Messages;

namespace Kengic.Was.Systems.Message
{
    public class MessageRepository
    {
        private const string MessageSection = "messageSection";
        private static string _defaultLanguage = "English";

        private static readonly ConcurrentDictionary<string, MessageElement>
            MessagesQueue =
                new ConcurrentDictionary<string, MessageElement>();

        public static void LoadMessageConfiguration(string fileName)
        {
            var configurations = ConfigurationOperation<MessageSection>.GetCustomSection(fileName,
                MessageSection);
            if (configurations == null)
            {
                TraceLogHelper.WriteTraceLog($"No Section,[{MessageSection}]");
                return;
            }

            foreach (MessageElement message in configurations.Messages)
            {
                MessagesQueue.TryAdd(message.Id, message);
            }
        }

        public static string GetMessage(string messageId) => GetMessage(_defaultLanguage, messageId);

        public static string GetMessage(string language, string messageId)
        {
            if (!MessagesQueue.ContainsKey(messageId))
            {
                return messageId;
            }
            MessageElement tMessage;
            var result = MessagesQueue.TryGetValue(messageId, out tMessage);
            if (!result)
            {
                return messageId;
            }
            var tLanguageList =
                tMessage.Languages.OfType<LanguageElement>().Where(e => e.Id == language).ToList();
            return tLanguageList.Any() ? tLanguageList[0].MessageInfo : messageId;
        }

        public static void SetDefaultLanguage(string defaultLanguage) => _defaultLanguage = defaultLanguage;
    }
}