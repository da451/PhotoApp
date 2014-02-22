using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DALC;
using MVVMPhotoApp.Extention;
using MVVMPhotoApp.Model;

namespace PhotoApp.Utils
{
    public static class ColorUtil
    {
        private static List<PColorModel> _knownColors = null;

        private static readonly object syncRoot = new object();

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

        public static PColorModel CompareColors(PColorModel color)
        {
            if (_knownColors == null)
            {
                _knownColors = FNHHelper.SelectAllPColors().ToModel().ToList();
            }

            return _knownColors.OrderBy(o => DeltaRGB(o, color)).Select(o=> new PColorModel(o.ColorID,o.Value,o.Name){Percent = color.Percent}).First();

        }

        public static IList<PColorModel> DictionaryToKnownPColorList(Dictionary<Color, double> colors)
        {
            var res = colors.Select(o => CompareColors(new PColorModel(o.Key.ToString(), string.Empty,o.Value))).ToList();
            return res;
        } 
    }
}

