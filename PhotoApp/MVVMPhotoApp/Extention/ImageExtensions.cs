using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALC.Entities;
using MVVMPhotoApp.Model;

namespace MVVMPhotoApp.Extention
{
    public static class ImageExtensions
    {
        public static ImageModel ToImageModel(this Image image)
        {
            ImageModel res = null;

            if (image != null)
            {
                byte[] imgBytes = new byte[0];

                if (image.Img != null)
                {
                    imgBytes = new byte[image.Img.Length];

                    image.Img.CopyTo(imgBytes, 0);
                }

                byte[] tmbBytes = new byte[0];

                if (image.Thumbnail != null)
                {
                    tmbBytes = new byte[image.Thumbnail.Length];

                    image.Thumbnail.CopyTo(tmbBytes, 0);
                }

                res = new ImageModel()
                {
                    ImageID = image.ImageID,
                    Img = imgBytes,
                    Thumbnail = tmbBytes,
                    Name = image.Name,
                    ImageColors = image.Colors.ToModel()
                };
            }

            return res;
        }
    }
}
