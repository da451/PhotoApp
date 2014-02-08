using System.Windows.Input;
using DALC;
using DALC.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PhotoApp;

namespace MVVMPhotoApp.Model
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ImageModel :   ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ImageModel class.
        /// </summary>
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

        public static explicit operator Image(ImageModel image)
        {
            byte[] imgBytes = new byte[image.Img.Length];

            byte[] tmbBytes = new byte[image.Thumbnail.Length];

            image.Img.CopyTo(imgBytes, 0);

            image.Thumbnail.CopyTo(tmbBytes, 0);

            return new Image(image.ImageID, imgBytes, tmbBytes, image.Name);
        }
    }
}