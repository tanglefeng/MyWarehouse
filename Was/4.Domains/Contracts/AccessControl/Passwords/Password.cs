using Kengic.Was.Domain.Entity.AccessControl.Users;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Passwords
{
    public class Password : EntityForTime<string>
    {
        public string HashCode { get; set; }
        public PasswordType PasswordType { get; set; }
        public int AuthenticationType { get; set; }
        public int PasswordDegree { get; set; }
        public string PasswordDefine { get; set; }
        public int MaxLoginTryNum { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
    }

    public enum PasswordType
    {
        LongType,
        ShortType,
        PinCode
    }
}