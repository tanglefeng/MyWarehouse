using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges;
using Kengic.Was.Domain.Entity.AccessControl.Passwords;
using Kengic.Was.Domain.Entity.AccessControl.Users;

namespace Kengic.Was.Application.Services.AccessControl.Passwords
{
    public class PasswordApplicationServices : IPasswordApplicationServices
    {
        private const string Code = "123456";
        private readonly IPasswordRepository _theRepository;
        private readonly IUserRepository _userRepository;

        public PasswordApplicationServices(IUserRepository userRepository,
            IPasswordRepository theRepository, IFunctionPrivilegeRepository functionprivilegeRepository)
        {
            if (theRepository == null)
            {
                throw new ArgumentNullException(nameof(theRepository));
            }
            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }
            if (functionprivilegeRepository == null)
            {
                throw new ArgumentNullException(nameof(functionprivilegeRepository));
            }

            _theRepository = theRepository;
            _userRepository = userRepository;
        }

        public Tuple<bool, string> Create(Password value)
        {
            if (string.IsNullOrEmpty(value.Id))
            {
                const string messageCode = StaticParameterForMessage.IdIsNull;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            var isAlreadyExist = _theRepository.TryGetValue(value.Id);
            if (isAlreadyExist != null)
            {
                const string messageCode = StaticParameterForMessage.ObjectIsExist;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            var passwordList = _theRepository.GetAll();
            foreach (var passwordoracle in passwordList)
            {
                if ((passwordoracle.UserId != value.UserId) || (passwordoracle.PasswordType != value.PasswordType))
                    continue;
                const string messageCode = StaticParameterForMessage.ObjectIsExist;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            if (string.IsNullOrEmpty(value.HashCode))
            {
                //新建时密码不能为空
                const string messageCode = StaticParameterForMessage.ValueIsNull;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            //将输入的密码进行MD5加密
            var md5Hash = MD5.Create();
            var hash = GetMd5Hash(md5Hash, value.HashCode);
            value.HashCode = hash;

            return _theRepository.Create(value);
        }

        public string GetMd5Hash(HashAlgorithm md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            var hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            var comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }

        public Tuple<bool, string, User> LoginOut(string userId)
        {
            var messageCode = StaticParameterForMessage.LoginSuccess;
            var user = _userRepository.GetAllWithId(userId);
            if (user != null) return new Tuple<bool, string, User>(true, messageCode, user);
            messageCode = StaticParameterForMessage.LoginFailure;
            return new Tuple<bool, string, User>(false, messageCode, null);
        }

        public Tuple<bool, string> ChangePassword(string userId, string originPassword, string newPassword)
        {
            var value = _theRepository.GetWithUserAndType(userId, 0);
            var md5Hash = MD5.Create();
            if (value == null)
            {
                return new Tuple<bool, string>(false, "wrong password!");
            }
            //validate password input to password record
            if (!VerifyMd5Hash(md5Hash, originPassword, value.HashCode))
            {
                return new Tuple<bool, string>(false, "wrong password!");
            }
            var hash = GetMd5Hash(md5Hash, newPassword);
            value.HashCode = hash;
            return _theRepository.Update(value);
        }

        public Tuple<bool, string> ResetPassword(string userId)
        {
            var messageCode = StaticParameterForMessage.ResetSuccess;
            var useroracle = _userRepository.GetAllWithId(userId);
            var passwordList = useroracle.Passwords;
            foreach (var password in passwordList)
            {
                password.HashCode = Code;
                var response = Update(password);
                messageCode = response.Item2;
                if (response.Item1 == false)
                {
                    return new Tuple<bool, string>(false, messageCode);
                }
            }
            return new Tuple<bool, string>(true, messageCode);
        }

        public Tuple<bool, string> Update(Password value)
        {
            if (string.IsNullOrEmpty(value.HashCode))
            {
                //如果密码为空，则表示不修改密码
                var hashCode = _theRepository.TryGetValue(value.Id).HashCode;
                value.HashCode = hashCode;
            }
            else
            {
                var md5Hash = MD5.Create();
                var hashCode = GetMd5Hash(md5Hash, value.HashCode);
                value.HashCode = hashCode;
            }
            return _theRepository.Update(value);
        }

        public Tuple<bool, string> Remove(Password value) => _theRepository.Remove(value);

        public Tuple<bool, string, User> Login(string userid, string password,
            int passwordtype)
        {
            var messageCode = StaticParameterForMessage.LoginFailure;
            var user = _userRepository.GetAllWithId(userid);
            if (user == null)
            {
                return new Tuple<bool, string, User>(false, messageCode, null);
            }
            var passwordDatabase = _theRepository.GetWithUserAndType(userid, passwordtype);
            var md5Hash = MD5.Create();
            //validate password input to password record
            if (!VerifyMd5Hash(md5Hash, password, passwordDatabase.HashCode))
                return new Tuple<bool, string, User>(false, messageCode, null);
            messageCode = StaticParameterForMessage.LoginSuccess;
            return new Tuple<bool, string, User>(true, messageCode, user);
        }

        public IQueryable<Password> GetAll() => _theRepository.GetAll();
    }
}