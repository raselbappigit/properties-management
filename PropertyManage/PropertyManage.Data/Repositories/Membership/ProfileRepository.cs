using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Domain;
using PropertyManage.Data;

namespace PropertyManage.Data
{
    public class ProfileRepository : RepositoryBase<Profile>, IProfileRepository
    {
        public ProfileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IProfileRepository : IRepository<Profile>
    {

    }
}
