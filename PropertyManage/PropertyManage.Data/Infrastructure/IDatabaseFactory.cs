using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyManage.Data
{
    public interface IDatabaseFactory : IDisposable
    {
        AppDbContext Get();
    }
}
