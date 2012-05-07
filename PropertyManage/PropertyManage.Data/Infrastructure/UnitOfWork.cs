using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyManage.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private AppDbContext _dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        protected AppDbContext DataContext
        {
            get { return _dataContext ?? (_dataContext = _databaseFactory.Get()); }
        }

        public void Commit()
        {
            DataContext.SaveChanges();
        }
    }
}
