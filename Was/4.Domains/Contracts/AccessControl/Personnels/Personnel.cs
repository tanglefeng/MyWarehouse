using Kengic.Was.Domain.Entity.AccessControl.Departments;
using Kengic.Was.Domain.Entity.AccessControl.Users;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Personnels
{
    public class Personnel : EntityForTime<string>
    {
        public Sex Sex { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public int Job { get; set; }
        public string MobileNo { get; set; }
        public string PostAddress { get; set; }
        public string CompanyTelephone { get; set; }
        public string Comments { get; set; }
        public string PhotoLink { get; set; }
        public virtual User User { get; set; }
        public string DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }

    public enum Sex
    {
        Male,
        Female
    }
}