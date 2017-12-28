using System.Collections.Generic;

namespace Kengic.Was.Application.WasModel.Dto.AccessControls.Personnels
{
    public class CompanyDto : EntityForTimeDto<string>
    {
        public string AddressDetail { get; set; }
        public string AddressCode { get; set; }
        public int IsdefaultAddress { get; set; }
        public string Fax { get; set; }
        public string Telephone { get; set; }
        public string EmailAddress { get; set; }
        public string EmailCode { get; set; }
        public string PhotoLink { get; set; }
        public string ParentId { get; set; }
        public string ParentCompanyName { get; set; }
        public List<CompanyDto> Children { get; set; }
    }
}