using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Data;
using PropertyManage.Domain;

namespace PropertyManage.Data
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IRoleRepository : IRepository<Role>
    {

    }
}
