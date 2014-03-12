using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALC.Entities;
using DALC.Interfaces;
using DALC.Repository;

namespace DALC
{
    static class Program
    {
        public static void Main(string[] p)
        {


            var uow = FNHHelper.CreateUoW();

            var colors = new RepositoryPColor(uow);

            var c = colors.Select().ToList();

            var images = new RepositoryImage(uow);

            var i = images.Get(143);

            i.AddColor(c[3]);

            uow.Commit();

            uow = FNHHelper.CreateUoW();

            images = new RepositoryImage(uow);

            i = images.Get(143);

            i.AddColor(c[4]);

            uow.Commit();


            uow = FNHHelper.CreateUoW();

            images = new RepositoryImage(uow);

            i = images.Get(143);

            i.Colors.Clear();

            uow.Commit();

        }
    }
}
