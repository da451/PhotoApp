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
            List<Image> images = FNHHelper.SelectAllImages();
        }
    }
}
