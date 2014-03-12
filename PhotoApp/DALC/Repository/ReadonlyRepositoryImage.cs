using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALC.Entities;

namespace DALC.Repository
{
    public class ReadonlyRepositoryImage : ReadonlyRepositoryBase<Image>
    {
        public ReadonlyRepositoryImage() :  base()
        {
        }
    }
}
