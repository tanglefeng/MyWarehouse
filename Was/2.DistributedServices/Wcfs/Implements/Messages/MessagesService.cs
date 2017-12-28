using System;
using Kengic.Was.Application.Services.Messages;
using Kengic.Was.Wcf.IMessages;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.Messages
{
    [ExceptionShielding("WcfServicePolicy")]
    public class MessagesService : IMessagesService
    {
        private readonly IMessagesApplicationService _messagesApplicationService;

        public MessagesService(IMessagesApplicationService messagesApplicationService)
        {
            if (messagesApplicationService == null)
            {
                throw new ArgumentNullException(nameof(messagesApplicationService));
            }
            _messagesApplicationService = messagesApplicationService;
        }

        public string GetValueByLanguage(string language, string messageCode)
            => _messagesApplicationService.GetValueByLanguage(language, messageCode);

        public string GetValue(string messageCode) => _messagesApplicationService.GetValueById(messageCode);
    }
}