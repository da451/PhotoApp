using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALC.Entities;

namespace DALC
{
    static class Program
    {
        public static void Main(string[] p)
        {
            List<PColor> colors = FNHHelper.SelectAllPColors();

            List<Image> images = FNHHelper.SelectAllImages();

            colors.First();

            Image s = images.First();

            //Image i = new Image();

            FNHHelper.CreateImage(s.Img, null, DateTime.Now.ToLongTimeString(), colors.OrderBy(o => o.ColorID).Take(3).ToList());


        }
    }
}
