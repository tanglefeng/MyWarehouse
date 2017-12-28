using Kengic.Was.Systems.Message;

namespace Kengic.Was.Application.Services.Messages
{
    public class MessagesApplicationService : IMessagesApplicationService
    {
        public string GetValueByLanguage(string language, string messageId)
            => MessageRepository.GetMessage(language, messageId);

        public string GetValueById(string messageId) => MessageRepository.GetMessage(messageId);
    }
}