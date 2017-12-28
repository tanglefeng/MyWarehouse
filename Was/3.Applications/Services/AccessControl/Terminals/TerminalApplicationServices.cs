using System;
using System.Linq;
using Kengic.Was.Domain.Entity.AccessControl.Terminals;

namespace Kengic.Was.Application.Services.AccessControl.Terminals
{
    public class TerminalApplicationServices : ITerminalApplicationServices
    {
        private readonly ITerminalRepository _theRepository;

        public TerminalApplicationServices(ITerminalRepository theRepository)
        {
            if (theRepository == null)
            {
                throw new ArgumentNullException(nameof(theRepository));
            }
            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(Terminal value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(Terminal value) => _theRepository.Update(value);
        public Tuple<bool, string> Remove(Terminal value) => _theRepository.Remove(value);
        public IQueryable<Terminal> GetAll() => _theRepository.GetAll();
    }
}