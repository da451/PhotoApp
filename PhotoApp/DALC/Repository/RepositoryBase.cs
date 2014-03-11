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
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity: IEntity 
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


        public IQueryable<TEntity> Select()
        {
            return Session.Query<TEntity>();
        }

        public TEntity Get(int id)
        {
            return Session.Get<TEntity>(id);
        }

        public int Insert(TEntity entity)
        {
            return Convert.ToInt32(Session.Save(entity));
        }

        public void Update(TEntity entity)
        {
            Session.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            Session.Delete(Session.Get<TEntity>(entity.PrimaryKey));
        }


        public TEntity Load(int id)
        {
            return Session.Load<TEntity>(id);

        }
    }
}
