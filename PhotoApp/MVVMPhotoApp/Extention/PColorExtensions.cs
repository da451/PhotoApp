using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALC.Entities;
using MVVMPhotoApp.Model;

namespace MVVMPhotoApp.Extention
{
    public static class PColorExtensions
    {
        public static PColorModel ToPColorModel(this PColor color)
        {
            PColorModel res = null;

            if (color != null)
            {
                res = new PColorModel()
                {
                    ColorID = color.ColorID,
                    Name = color.Name,
                    Value = color.Value
                };
            }

            return res;
        }
    }
}
