using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using AForge.Imaging.ColorReduction;
using MVVMPhotoApp.Extention;
using Color = System.Windows.Media.Color;

namespace MVVMPhotoApp.Utils
{
    public static class AForgeUtil
    {
        private static int _paletteColorCount = 10;

        public static BitmapImage ImageQuantizer(BitmapImage image)
        {
            ColorImageQuantizer ciq = new ColorImageQuantizer(new MedianCutQuantizer());

            Bitmap bitmap = ImageUtils.BitmapImageToBitmap(image);

            Bitmap reducedImage = ciq.ReduceColors(bitmap, _paletteColorCount);

            return ImageUtils.BitmapToBitmapImage(reducedImage);
        }

        public static IList<Color> GetImagePalette(BitmapImage image)
        {
            ColorImageQuantizer ciq = new ColorImageQuantizer(new MedianCutQuantizer());

            Bitmap bitmap = ImageUtils.BitmapImageToBitmap(image);

            Color[] colors = ciq.CalculatePalette(bitmap, _paletteColorCount).Select(o=> o.ToMediaColor()).ToArray();

            return colors.ToList();
        }

        public static void GetImageDomainColors(BitmapImage image)
        {
            Dictionary<Color, int> domainColors = new Dictionary<Color, int>();

            BitmapImage img = ImageQuantizer(image);

            ImageUtils.SaveImage(img);

            int height = img.PixelHeight;

            int width = img.PixelWidth;

            int nStride = (width * img.Format.BitsPerPixel + 7) / 8;

            byte[] pixelByteArray = new byte[height*nStride];

            img.CopyPixels(pixelByteArray, nStride, 0);

            domainColors.Clear();

            //for (int pixelNumber = 0; pixelNumber < pixelByteArray.Length-1; pixelNumber += 4)
            //{
            //    byte red = pixelByteArray[pixelNumber+2];

            //    byte green = pixelByteArray[pixelNumber+1];

            //    byte blue = pixelByteArray[pixelNumber + 0];

            //    Color c = Color.FromArgb(red, green, blue);

            //    if (domainColors.ContainsKey(c))
            //    {
            //        domainColors[c]++;
            //    }
            //    else
            //    {
            //        domainColors.Add(c,1);
            //    }
            //}
        }
    }
}

