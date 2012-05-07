using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyManage.Data
{
   public class DatabaseFactory : Disposable, IDatabaseFactory
    {
       private AppDbContext _dataContext;
       public AppDbContext Get()
        {
            return _dataContext ?? (_dataContext = new AppDbContext());
        }
        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
    }
}
