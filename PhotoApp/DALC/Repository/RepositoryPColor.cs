using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALC.Entities;
using DALC.Interfaces;

namespace DALC.Repository
{
    public class RepositoryPColor : RepositoryBase<PColor>,IRepositoryPColor
    {
        public RepositoryPColor(IUnitOfWork uow) : base(uow)
        {
        }

        public void Insert(string name, string value)
        {
            Insert(new PColor(value,name));
        }
    }
}
