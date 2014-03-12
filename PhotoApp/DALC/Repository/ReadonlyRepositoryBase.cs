using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALC.Interfaces;
using NHibernate;
using NHibernate.Linq;

namespace DALC.Repository
{
    public class ReadonlyRepositoryBase<T> : IReadonlyRepository<T> where T: IEntity
    {
        private ISessionFactory _sessionFactory;


        public ReadonlyRepositoryBase()
        {
            _sessionFactory = FNHHelper.SessionFactory;
        }


        public IList<T> Select()
        {
            ISession session = _sessionFactory.OpenSession();

            IList<T> res = session.Query<T>().ToList();

            session.Close();

            return res;
        }

        public T Get(int id)
        {
            ISession session = _sessionFactory.OpenSession();

            T res = session.Get<T>(id);

            session.Close();

            return res;
        }

        public T Load(int id)
        {
            ISession session = _sessionFactory.OpenSession();

            T res = session.Load<T>(id);

            session.Close();

            return res;
        }
    }
}
