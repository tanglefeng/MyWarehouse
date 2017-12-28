namespace Kengic.Was.Application.Services.Messages
{
    public interface IMessagesApplicationService
    {
        string GetValueByLanguage(string language, string messageId);
        string GetValueById(string messageId);
    }
}