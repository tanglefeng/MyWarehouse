using Kengic.Was.Domain.Entity.AccessControl.Personnels;

namespace Kengic.Was.Application.WasModel.Dto.AccessControls.Personnels
{
    public class PersonnelDto : EntityForTimeDto<string>
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
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}