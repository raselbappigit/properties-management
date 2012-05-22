using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Domain;

namespace PropertyManage.Data
{
    public class UnitTypeRepository : RepositoryBase<UnitType>, IUnitTypeRepository
    {
        public UnitTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
    public interface IUnitTypeRepository : IRepository<UnitType>
    {

    }

}
