using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Domain;

namespace PropertyManage.Data
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IProjectRepository : IRepository<Project>
    {

    }
}
