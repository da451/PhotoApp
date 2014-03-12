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
    public class RepositoryBase<T> : IRepository<T> where T: IEntity 
    {

        protected ISession Session
        {
            get;
            private set;
        }

        private IUnitOfWork _unitOfWork;

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public RepositoryBase(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            Session = _unitOfWork.Session;
        }


        public IQueryable<T> Select()
        {
            return Session.Query<T>();
        }

        public T Get(int id)
        {
            return Session.Get<T>(id);
        }

        public int Insert(T entity)
        {
            return Convert.ToInt32(Session.Save(entity));
        }

        public void Update(T entity)
        {
            Session.Update(entity);
        }

        public void Delete(T entity)
        {
            Session.Delete(Session.Get<T>(entity.PrimaryKey));
        }

        public T Load(int id)
        {
            return Session.Load<T>(id);

        }
    }
}
