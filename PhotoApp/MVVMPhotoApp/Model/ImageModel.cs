using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DALC;
using DALC.Entities;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MVVMPhotoApp.Extention;
using MVVMPhotoApp.Utils;
using PhotoApp;
using PhotoApp.Utils;

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



        public const string ImageBitmapPropertyName = "ImageBitmap";

        private BitmapImage _imageBitmap;

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
                IsLoaded = _imageColors.Any();
            }
        }



        public const string IsLoadedPropertyName = "IsLoaded";

        private bool _isLoaded = false;

        public bool IsLoaded
        {
            get
            {
                return _isLoaded;
            }

            set
            {
                if (_isLoaded == value)
                {
                    return;
                }

                RaisePropertyChanging(IsLoadedPropertyName);
                _isLoaded = value;
                RaisePropertyChanged(IsLoadedPropertyName);
            }
        }

        #region Methods

        public void Save()
        {
            this._imageID = FNHHelper.CreateImage(this.Img, null, Name);

            Task taskFindDomainColors = new Task((model) =>
            {
                ImageModel imageModel = (ImageModel) model; //FNHHelper.SelectImagesByID((int)id).ToImageModel();

                Dictionary<Color, double> colorDic = AForgeUtil.ImageQuantizerByte(imageModel.Img, 3);

                imageModel.ImageColors = new ObservableCollection<PColorModel>(ColorUtil.DictionaryToKnownPColorList(colorDic));

                FNHHelper.UpdateImage((DALC.Entities.Image)imageModel);

            }, this);

            taskFindDomainColors.Start();
        }

        public bool Delete()
        {
            return FNHHelper.DeleteImage(this.ImageID);
        }

        #endregion


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