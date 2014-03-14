using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Media.Imaging;
using DALC;
using DALC.Entities;
using DALC.Repository;
using GalaSoft.MvvmLight;
using MVVMPhotoApp.Extention;
using MVVMPhotoApp.Manager;
using Timer = System.Timers.Timer;


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
            RepositoryImage repositoryImage =
                new RepositoryImage(FNHHelper.CreateUoW());

            this._imageID = repositoryImage.Insert(new Image()
            {
                Img = this.Img,
                Name = this.Name
            });

            repositoryImage.UnitOfWork.Commit();

            ImageManager.Instance.FindColors(this);
        }

        public bool Delete()
        {
            RepositoryImage repositoryImage = new RepositoryImage(FNHHelper.CreateUoW());

            repositoryImage.Delete((DALC.Entities.Image) this);

            repositoryImage.UnitOfWork.Commit();

            return true;
        }

        private Timer _cheakTimer = null;

        public void Cheak()
        {
            if (this.ImageID > 0 && !this.IsLoaded && _cheakTimer == null)
            {
                _cheakTimer = new Timer(3000);

                _cheakTimer.Elapsed += CheakTimerOnElapsed;

                _cheakTimer.Start();
            }

        }

        private void CheakTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            ReadonlyRepositoryImage readonlyRepositoryImage =
                new ReadonlyRepositoryImage();

            Image img = readonlyRepositoryImage.Get(this.ImageID);

            if (img.Colors != null && img.Colors.Count != 0)
            {
                this.ImageColors = img.Colors.ToModel();

                _cheakTimer.Stop();

                _cheakTimer = null;
            }
        }

        #endregion


        public static explicit operator Image(ImageModel image)
        {
            byte[] imgBytes = new byte[image.Img.Length];

            byte[] tmbBytes = null;

            image.Img.CopyTo(imgBytes, 0);

            if (image.Thumbnail != null)
            {
                tmbBytes = new byte[image.Thumbnail.Length];
                image.Thumbnail.CopyTo(tmbBytes, 0);
            }

            return new Image(image.ImageID, imgBytes, tmbBytes, image.Name, image.ImageColors.ToEntity());
        }
    }
}