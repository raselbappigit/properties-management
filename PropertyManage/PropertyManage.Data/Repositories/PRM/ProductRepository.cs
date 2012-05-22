using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Domain;

namespace PropertyManage.Data
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IProductRepository : IRepository<Product>
    {

    }
}
