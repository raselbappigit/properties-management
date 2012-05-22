using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Domain;

namespace PropertyManage.Data
{
    public class UnitValueRepository : RepositoryBase<UnitValue>, IUnitValueRepository
    {
        public UnitValueRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IUnitValueRepository : IRepository<UnitValue>
    {

    }

}
