using System.Collections.Generic;
using System.Linq;

namespace DALC.Interfaces
{
    public interface IReadonlyRepository<T> where T : IEntity
    {
        IList<T> Select();
        T Get(int id);
        T Load(int id);
    }
}