using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
  
        public static byte[] ImageQuantizerByte(BitmapImage image)
        {
            ColorImageQuantizer ciq = new ColorImageQuantizer(new MedianCutQuantizer());

            Bitmap bitmap = ImageUtils.BitmapImageToBitmap(image);

            Bitmap reducedImage = ciq.ReduceColors(bitmap, _paletteColorCount);

            ImageConverter converter = new ImageConverter();

            return (byte[])converter.ConvertTo(reducedImage, typeof(byte[]));
        }

        public static Dictionary<Color, int> ImageQuantizerByte(BitmapImage image, int colorCount , out byte[] byteImage)
        {
            ColorImageQuantizer ciq = new ColorImageQuantizer(new MedianCutQuantizer());

            Bitmap bitmap = ImageUtils.BitmapImageToBitmap(image);

            Bitmap reducedImage = ciq.ReduceColors(bitmap, _paletteColorCount);

            ImageConverter converter = new ImageConverter();

            byteImage = (byte[])converter.ConvertTo(reducedImage, typeof(byte[]));
            
            Rectangle rect = new Rectangle(0, 0, reducedImage.Width, reducedImage.Height);

            BitmapData bmpData = reducedImage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = bmpData.Stride*reducedImage.Height;

            byte[] rgbValues = new byte[bytes];

// Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Dictionary<Color, int> pixelsCount = new Dictionary<Color, int>();

            int stride = bmpData.Stride;

            for (int column = 0; column < bmpData.Height; column++)
            {
                for (int row = 0; row < bmpData.Width; row++)
                {
                    byte blue = (byte) (rgbValues[(column*stride) + (row*3)]);

                    byte green = (byte) (rgbValues[(column*stride) + (row*3) + 1]);

                    byte red = (byte) (rgbValues[(column*stride) + (row*3) + 2]);

                    Color c = Color.FromRgb(red, green, blue);

                    if (!pixelsCount.ContainsKey(c))
                    {
                        pixelsCount.Add(c,1);
                    }
                    else
                    {
                        pixelsCount[c]++;
                    }

                }
            }
            var res = pixelsCount.OrderByDescending(o => o.Value)
                .Take(colorCount)
                .ToDictionary(k => k.Key, v => v.Value);
            return  res;
        }

        public static Dictionary<Color, double> ImageQuantizerByte(byte[] image, int colorCount)
        {
            ImageConverter ic = new ImageConverter();

            ColorImageQuantizer ciq = new ColorImageQuantizer(new MedianCutQuantizer());

            Bitmap reducedImage = ciq.ReduceColors(new Bitmap((Image)ic.ConvertFrom(image)), _paletteColorCount);

            Rectangle rect = new Rectangle(0, 0, reducedImage.Width, reducedImage.Height);

            BitmapData bmpData = reducedImage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = bmpData.Stride * reducedImage.Height;

            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Dictionary<Color, int> pixelsCount = new Dictionary<Color, int>();

            int stride = bmpData.Stride;

            object syncRoot = new object();
            Parallel.For(0, bmpData.Height, column =>
            {
                try
                {
                    for (int row = 0; row < bmpData.Width; row++)
                    {
                        byte blue = (byte) (rgbValues[(column*stride) + (row*3)]);

                        byte green = (byte) (rgbValues[(column*stride) + (row*3) + 1]);

                        byte red = (byte) (rgbValues[(column*stride) + (row*3) + 2]);

                        Color c = Color.FromRgb(red, green, blue);
                        lock (syncRoot)
                        {
                            if (!pixelsCount.ContainsKey(c))
                            {
                                pixelsCount.Add(c, 1);
                            }
                            else
                            {
                                pixelsCount[c]++;
                            }
                        }

                    }
                }
                catch
                {
                    
                }

            });
            
            var topColors = pixelsCount.OrderByDescending(o => o.Value)
                .Take(colorCount)
                .ToDictionary(k => k.Key, v => v.Value);

            int sum = topColors.Sum(o => o.Value);

            var res = topColors.ToDictionary(k => k.Key, v => (v.Value*100.0)/sum);

            return res;
        }

        public static Dictionary<Color, double> ImageQuantizerByte(byte[] image, Point position, Size size ,int colorCount)
        {
            Bitmap finalImage = new Bitmap(size.Width, size.Height);

            System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(position,size);

            using (Bitmap sourceImage = ImageUtils.BytesToBitmap(image))
            {
                using (Bitmap croppedImage = sourceImage.Clone(cropRect, sourceImage.PixelFormat))
                {
                    using (TextureBrush tb = new TextureBrush(croppedImage))
                    {
                        using (Graphics g = Graphics.FromImage(finalImage))
                        {
                            g.FillRectangle(tb, 0, 0,size.Width,size.Height);
                        }
                    }
                }

            }

            return ImageQuantizerByte(ImageUtils.BitmapToBytes(finalImage), colorCount);
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

