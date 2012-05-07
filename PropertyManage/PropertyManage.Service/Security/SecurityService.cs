using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using PropertyManage.Domain;
using System.Web.Security;
using PropertyManage.Data;

namespace PropertyManage.Service
{
    public interface ISecurityService
    {
        IEnumerable<User> GetUsers();
        //IEnumerable<User> GetLoggedInUsers();

        User GetUser(string userName);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(string userName);

        //void ChangeUserLogInStatus(User user, bool inout);

        IEnumerable<Role> GetRoles();
        Role GetRole(string roleName);
        IEnumerable<User> GetUsersInRole(string roleName);

        void CreateRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(string roleName);

        void AddUserToRole(string userName, List<string> roleNames);

        bool ForgetPassword(User user);
        string HashResetParams(string username);

        void Save();
    }

    public class SecurityService : ISecurityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SecurityService(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<User> GetUsers()
        {
            var users = _userRepository.GetAll();
            return users;
        }

        //public IEnumerable<User> GetLoggedInUsers()
        //{
        //    var users = userRepository.GetAll().Where(x => x.IsLoggedIn == true).ToList();
        //    return users;
        //}

        public User GetUser(string userName)
        {
            var user = _userRepository.GetById(userName);
            return user;
        }

        public void CreateUser(User user)
        {
            _userRepository.Add(user);
            Save();
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
            Save();
        }

        public void DeleteUser(string userName)
        {
            var user = GetUser(userName);
            _userRepository.Delete(user);
            Save();
        }

        //public void ChangeUserLogInStatus(User user, bool inout)
        //{
        //    User userDb = userRepository.GetById(user.UserName);
        //    if (userDb != null)
        //    {
        //        userDb.IsLoggedIn = inout;
        //        userRepository.Update(userDb);
        //        Save();
        //    }
        //}

        public IEnumerable<Role> GetRoles()
        {
            var roles = _roleRepository.GetAll();
            return roles;
        }

        public Role GetRole(string roleName)
        {
            var role = _roleRepository.GetById(roleName);
            return role;
        }

        public IEnumerable<User> GetUsersInRole(string roleName)
        {
            List<User> users = null;
            var role = _roleRepository.GetById(roleName);

            if (role != null)
            {
                users = _roleRepository.GetById(roleName).Users.ToList();
            }

            return users;
        }

        public void CreateRole(Role role)
        {
            _roleRepository.Add(role);
            Save();
        }

        public void UpdateRole(Role role)
        {
            _roleRepository.Update(role);
            Save();
        }

        public void DeleteRole(string roleName)
        {
            var role = GetRole(roleName);
            _roleRepository.Delete(role);
            Save();
        }

        public void AddUserToRole(string userName, List<string> roleNames)
        {
            _userRepository.AssignRole(userName, roleNames);
            Save();
        }

        #region Reset Password

        //Reset Password
        public bool ForgetPassword(User user)
        {
            try
            {
                if (!String.IsNullOrEmpty(user.Email))
                {
                    SendResetEmail(user);
                    return true;
                }

                return false;

            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        //Send Email Method
        public void SendResetEmail(User user)
        {
            //string mailBody = GetResetEmailBody(user.UserName);
            //const string mailSubject = "Your new password";
            //const string mailFrom = "rasel@syntechbd.com";
            //string mailTo = user.Email;
            //MailService mailService = new MailService();

            //MailServiceResult mailResult = mailService.SendEmail(mailTo, mailFrom, mailSubject, mailBody);
        }

        public string HashResetParams(string username)
        {
            byte[] bytesofLink = Encoding.UTF8.GetBytes(username);
            MD5 md5 = new MD5CryptoServiceProvider();
            string hashParams = BitConverter.ToString(md5.ComputeHash(bytesofLink));

            return hashParams;
        }

        private string GetResetEmailBody(string userName)
        {
            string link = "http://www.syntechbd.com/Account/ResetPassword/?username=" + userName + "&reset=" + HashResetParams(userName);

            string mailBody = "<p>Hi " + userName + "," + "</p>";
            mailBody += "<p>You just requested a new password for your my regfire account.</p>";
            mailBody += "---------------------------------------------------------------------------</br>";
            mailBody += "For creating a new password please click on the following link:</br>";
            mailBody += "<a href='" + link + "'>" + link + "</a></br>";
            mailBody += "---------------------------------------------------------------------------</br>";
            mailBody += "<p>Please keep your password safe to prevent unathorized access.</p>";
            mailBody += "---------------------</br>";
            mailBody += "Property Management</br>";
            mailBody += "Our Slogan</br>";
            mailBody += "---------------------</br>";
            mailBody += @"<p>http://www.syntechbd.com/</p>";
            mailBody += @"<p>&copy; " + DateTime.Now.Year + " - Syntech BD | Syntech Ltd. - Bangladesh</p>";

            return mailBody;
        }

        #endregion

        public void Save()
        {
            _unitOfWork.Commit();
        }

    }
}

