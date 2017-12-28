using System.Collections.Generic;
using Kengic.Was.Domain.Entity.AccessControl.Departments;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Companys
{
    public class Company : EntityForTime<string>
    {
        public string AddressDetail { get; set; }
        public string AddressCode { get; set; }
        public int IsdefaultAddress { get; set; }
        public string Fax { get; set; }
        public string Telephone { get; set; }
        public string EmailAddress { get; set; }
        public string EmailCode { get; set; }
        public string ParentId { get; set; }
        public virtual Company Parent { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public string PhotoLink { get; set; }
    }
}