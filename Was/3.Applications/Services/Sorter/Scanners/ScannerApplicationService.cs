using System;
using System.Linq;
using Kengic.Was.Domain.Entity.Sorter.Scanners;

namespace Kengic.Was.Application.Services.Sorter.Scanners
{
    public class ScannerApplicationService : IScannerApplicationService
    {
        private readonly IScannerRepository _theRepository;

        public ScannerApplicationService(
            IScannerRepository theRepository)
        {
            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(Scanner value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(Scanner value) => _theRepository.Update(value);
        public Tuple<bool, string> Remove(Scanner value) => _theRepository.Remove(value);
        public IQueryable<Scanner> GetAll() => _theRepository.GetAll();
    }
}