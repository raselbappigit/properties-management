using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Data;
using PropertyManage.Domain;
using System.Data;

namespace PropertyManage.Data
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public void AssignRole(string userName, List<string> roleNames)
        {
            User user = this.GetById(userName);
            if (user != null)
            {
                user.Roles.Clear();
                foreach (string roleName in roleNames)
                {
                    var role = this.DataContext.Roles.Find(roleName);
                    user.Roles.Add(role);
                }

                this.DataContext.Users.Attach(user);
                this.DataContext.Entry(user).State = EntityState.Modified;
            }
        }

    }

    public interface IUserRepository : IRepository<User>
    {
        void AssignRole(string userName, List<string> roleName);
    }
}
