using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALC.Interfaces;
using NHibernate;

namespace DALC.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ISession _session;

        public ISession Session
        {
            get
            {
                return _session;
            }
            private set
            {
                _session = value;
            }
            
        }

        private ISessionFactory _sessionFactory;

        private ITransaction _transaction;

        public UnitOfWork(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            BeginTransaction();
        }

        public UnitOfWork()
        {
            _sessionFactory = FNHHelper.SessionFactory;
            BeginTransaction();
        }


        private void BeginTransaction()
        {
            Session = _sessionFactory.OpenSession();
            _transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            finally
            {
                Session.Close();                
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction.Rollback();
            }
            finally
            {
                Session.Close();                
            }
        }

        void IDisposable.Dispose()
        {
            _sessionFactory = null;
            Session.Dispose();
            Session = null;
        }
    }
}

