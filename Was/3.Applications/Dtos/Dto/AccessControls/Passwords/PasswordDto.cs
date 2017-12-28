using Kengic.Was.Domain.Entity.AccessControl.Passwords;

namespace Kengic.Was.Application.WasModel.Dto.AccessControls.Passwords
{
    public class PasswordDto : EntityForTimeDto<string>
    {
        public string HashCode { get; set; }
        public PasswordType PasswordType { get; set; }
        public int AuthenticationType { get; set; }
        public int PasswordDegree { get; set; }
        public string PasswordDefine { get; set; }
        public int MaxLoginTryNum { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}