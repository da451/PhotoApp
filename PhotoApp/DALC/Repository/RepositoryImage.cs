using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALC.Entities;
using DALC.Interfaces;

namespace DALC.Repository
{
    public class RepositoryImage : RepositoryBase<Image>, IRepositoryImage
    {
        public RepositoryImage(IUnitOfWork uow) : base(uow)
        {
        }

    }
}
