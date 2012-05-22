using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Domain;

namespace PropertyManage.Data
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface ICategoryRepository : IRepository<Category>
    {

    }
}
