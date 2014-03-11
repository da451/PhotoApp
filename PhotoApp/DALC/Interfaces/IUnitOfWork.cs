using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace DALC.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISession Session { get;}
        void Commit();
        void Rollback();
    }
}
