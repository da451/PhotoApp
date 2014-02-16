using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MVVMPhotoApp.Model;

namespace PhotoApp.Utils
{
    public static class ColorUtil
    {
        public static double DeltaRGB(PColorModel firstPColor, PColorModel secondPColor)
        {
            Color firstColor = (Color) ColorConverter.ConvertFromString(firstPColor.Value);

            Color secondColor = (Color) ColorConverter.ConvertFromString(secondPColor.Value);

            double deltaR = (double) (firstColor.R - secondColor.R);

            double deltaG = (double) (firstColor.G - secondColor.G);

            double deltaB = (double) (firstColor.B - secondColor.B);

            return (Math.Sqrt(deltaR*deltaR + deltaG*deltaG + deltaB*deltaB));
        }

        public static IList<PColorModel> CompareColors(List<PColorModel> colors, PColorModel color, int count)
        {
            var r = colors.OrderBy(o => DeltaRGB(o,color)).Take(count).ToList();
                //var r = colors.Select(o => new {PColor = o, Delta = DeltaRGB(o, color)}).OrderBy(o => o.Delta).Take(5);
            return (IList<PColorModel>)r;

        }

    }
}

