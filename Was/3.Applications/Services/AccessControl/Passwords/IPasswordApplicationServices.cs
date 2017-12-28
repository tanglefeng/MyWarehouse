using System;
using System.Security.Cryptography;
using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.AccessControl.Passwords;
using Kengic.Was.Domain.Entity.AccessControl.Users;

namespace Kengic.Was.Application.Services.AccessControl.Passwords
{
    public interface IPasswordApplicationServices :
        IEditApplicationService<Password>, IQueryApplicationService<Password>
    {
        Tuple<bool, string, User> Login(string username, string password, int passwordtype);
        string GetMd5Hash(HashAlgorithm md5Hash, string input);
        bool VerifyMd5Hash(MD5 md5Hash, string input, string hash);
        Tuple<bool, string, User> LoginOut(string usreId);
        Tuple<bool, string> ChangePassword(string userId, string originPassword, string newPassword);
        Tuple<bool, string> ResetPassword(string userId);
    }
}