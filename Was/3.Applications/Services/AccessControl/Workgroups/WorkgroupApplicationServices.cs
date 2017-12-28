using System;
using System.Linq;
using Kengic.Was.Domain.Entity.AccessControl.Workgroups;

namespace Kengic.Was.Application.Services.AccessControl.Workgroups
{
    public class WorkgroupApplicationServices : IWorkgroupApplicationServices
    {
        private readonly IWorkgroupRepository _theRepository;

        public WorkgroupApplicationServices(IWorkgroupRepository theRepository)
        {
            if (theRepository == null)
            {
                throw new ArgumentNullException(nameof(theRepository));
            }
            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(Workgroup value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(Workgroup value) => _theRepository.Update(value);
        public Tuple<bool, string> Remove(Workgroup value) => _theRepository.Remove(value);
        public IQueryable<Workgroup> GetAll() => _theRepository.GetAll();
    }
}