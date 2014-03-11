using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALC.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        IQueryable<T> Select();
        T Get(int id);
        T Load(int id);
        int Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
