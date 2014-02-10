using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MVVMPhotoApp.Converter
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            byte[] imageBytes = value as byte[];

            BitmapImage image = null;

            if (imageBytes != null)
            {
                image = Utils.ImageUtils.BytesToImage(imageBytes);
            }
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            byte[] imageBytes =null;

            BitmapImage image = value as BitmapImage;

            if (image != null)
            {
                imageBytes = Utils.ImageUtils.ImageToBytes(image);
            }
            return imageBytes;
        }
    }
}
