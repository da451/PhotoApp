using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MVVMPhotoApp.Utils
{
    public static class ImageUtils
    {
        public static byte[] ImageToBytes(BitmapImage btm)
        {
            byte[] bytes;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(btm));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);

                bytes = ms.ToArray();
            }

            return bytes;
        }

        public static BitmapImage BytesToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(bytes))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new JpegBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

                encoder.Save(memoryStream);

                Bitmap bitmap = new Bitmap(memoryStream);

                return new Bitmap(bitmap);
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();

            BitmapSource bitmapSource;

            try
            {
                bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return BitmapSourceToBitmapImage(bitmapSource);
        }

        public static BitmapImage BitmapSourceToBitmapImage(BitmapSource bitmapSource)
        {
            BitmapImage bitmapImage = new BitmapImage();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                encoder.Save(memoryStream);

                bitmapImage.BeginInit();

                bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());

                bitmapImage.EndInit();

            }
            string path = string.Format(@"C:\{0}_{1}_{2}_{3}.jpg", DateTime.Now.Hour, DateTime.Now.Minute,
                DateTime.Now.Second, DateTime.Now.Millisecond);
            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew))
            {

                JpegBitmapEncoder encoder = new JpegBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                encoder.Save(fileStream);
            }

            return bitmapImage;
        }

        public static BitmapImage BitmapImageFromFile(string path)
        {
            BitmapImage bitmapImage = new BitmapImage();

            var bitmapFrame = BitmapFrame.Create(new Uri(path));

            int h = bitmapFrame.PixelHeight;

            int w = bitmapFrame.PixelWidth;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();

                encoder.Frames.Add(bitmapFrame);
                
                encoder.Save(memoryStream);

                bitmapImage.BeginInit();

                bitmapImage.DecodePixelHeight =h/2;

                bitmapImage.DecodePixelHeight =w/2;

                bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());

                bitmapImage.EndInit();

            }
            string pathFile = string.Format(@"C:\{0}_{1}_{2}_{3}.jpg", DateTime.Now.Hour, DateTime.Now.Minute,
                DateTime.Now.Second, DateTime.Now.Millisecond);
            using (FileStream fileStream = new FileStream(pathFile, FileMode.CreateNew))
            {

                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

                encoder.Save(fileStream);
            }

            return bitmapImage;
        }
    }
}
