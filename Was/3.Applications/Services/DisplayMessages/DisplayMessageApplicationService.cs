using System;
using System.Linq;
using Kengic.Was.Domain.Entity.DisplayMessage;

namespace Kengic.Was.Application.Services.DisplayMessages
{
    public class DisplayMessageApplicationService : IDisplayMessageApplicationService
    {
        private readonly IDisplayMessageRepository _theRepository;

        public DisplayMessageApplicationService(IDisplayMessageRepository theRepository)
        {
            if (theRepository == null)
            {
                throw new ArgumentNullException(nameof(theRepository));
            }
            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(DisplayMessage value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(DisplayMessage value) => _theRepository.Update(value);
        public Tuple<bool, string> Remove(DisplayMessage value) => _theRepository.Remove(value);
        public IQueryable<DisplayMessage> GetAll() => _theRepository.GetAll();
    }
}