using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DALC;
using DALC.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MVVMPhotoApp.Extention;
using PhotoApp;

namespace MVVMPhotoApp.Model
{

    public class ImageModel :   ViewModelBase
    {

        public ImageModel()
        {
        }

        public ImageModel(int imageID, byte[] img, byte[] thumbnail, string name): this()
        {
            ImageID = imageID;
            Img = img;
            Thumbnail = thumbnail;
            Name = name;

        }

        public ImageModel(byte[] img, byte[] thumbnail, string name)
            : this()
        {
            Img = img;
            Thumbnail = thumbnail;
            Name = name;

        }

        
        
        public const string ImageIDPropertyName = "ImageID";

        private int _imageID;

        public int ImageID
        {
            get
            {
                return _imageID;
            }

            set
            {
                if (_imageID == value)
                {
                    return;
                }

                RaisePropertyChanging(ImageIDPropertyName);
                _imageID = value;
                RaisePropertyChanged(ImageIDPropertyName);
            }
        }

        
        
        public const string ImgPropertyName = "Img";

        private byte[] _img;

        public byte[] Img
        {
            get
            {
                return _img;
            }

            set
            {
                if (_img == value)
                {
                    return;
                }

                RaisePropertyChanging(ImgPropertyName);
                _img = value;
                RaisePropertyChanged(ImgPropertyName);
            }
        }

        
        
        public const string ThumbnailPropertyName = "Thumbnail";

        private byte[] _thumbnail;

        public byte[] Thumbnail
        {
            get
            {
                return _thumbnail;
            }

            set
            {
                if (_thumbnail == value)
                {
                    return;
                }

                RaisePropertyChanging(ThumbnailPropertyName);
                _thumbnail = value;
                RaisePropertyChanged(ThumbnailPropertyName);
            }
        }

        
        
        public const string NamePropertyName = "Name";

        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name == value)
                {
                    return;
                }

                RaisePropertyChanging(NamePropertyName);
                _name = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }


        /// <summary>
        /// The <see cref="ImageBitmap" /> property's name.
        /// </summary>
        public const string ImageBitmapPropertyName = "ImageBitmap";

        private BitmapImage _imageBitmap;

        /// <summary>
        /// Sets and gets the ImageBitmap property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BitmapImage ImageBitmap
        {
            get
            {
                return _imageBitmap;
            }

            set
            {
                if (_imageBitmap == value)
                {
                    return;
                }

                RaisePropertyChanging(ImageBitmapPropertyName);
                _imageBitmap = value;
                RaisePropertyChanged(ImageBitmapPropertyName);
            }
        }


        public const string ImageColorsPropertyName = "ImageColors";

        private ObservableCollection<PColorModel> _imageColors;

        public ObservableCollection<PColorModel> ImageColors
        {
            get
            {
                return _imageColors;
            }

            set
            {
                if (_imageColors == value)
                {
                    return;
                }

                RaisePropertyChanging(ImageColorsPropertyName);
                _imageColors = value;
                RaisePropertyChanged(ImageColorsPropertyName);
            }
        }

        
        
        public static explicit operator Image(ImageModel image)
        {
            byte[] imgBytes = new byte[image.Img.Length];

            byte[] tmbBytes = new byte[image.Thumbnail.Length];

            image.Img.CopyTo(imgBytes, 0);

            image.Thumbnail.CopyTo(tmbBytes, 0);

            return new Image(image.ImageID, imgBytes, tmbBytes, image.Name, image.ImageColors.ToEntity());
        }
    }
}