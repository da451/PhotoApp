using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static byte[] ImageToBytes(Bitmap btm)
        {
            byte[] bytes;

            using (MemoryStream memory = new MemoryStream())
            {
                btm.Save(memory, ImageFormat.Jpeg);

                memory.Position = 0;

                bytes = memory.ToArray();
            }

            return bytes;
        }

        public static BitmapImage ImageFromBytes(byte[] bytes)
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

    }



}
