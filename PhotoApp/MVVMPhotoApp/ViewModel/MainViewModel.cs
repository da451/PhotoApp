using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using DALC;
using DALC.Entities;
using DALC.Repository;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMPhotoApp.Extention;
using MVVMPhotoApp.Model;
using MVVMPhotoApp.Notifications;
using MVVMPhotoApp.Utils;

namespace MVVMPhotoApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        public MainViewModel(IDataService dataService)
        {
            //_dataService = dataService;
            //_dataService.GetData(
            //    (item, error) =>
            //    {
            //        if (error != null)
            //        {
            //            // Report error here
            //            return;
            //        }

            //        WelcomeTitle = item.Title;
            //    });
        }

        private readonly object syncRoot = new object();


        #region ImageCollection
        public const string ImageCollectionPropertyName = "ImageCollection";

        private ObservableCollection<ImageModel> _imageCollection = new ObservableCollection<ImageModel>();

        public ObservableCollection<ImageModel> ImageCollection
        {
            get
            {
                return _imageCollection;
            }

            set
            {
                if (_imageCollection == value)
                {
                    return;
                }

                RaisePropertyChanging(ImageCollectionPropertyName);
                _imageCollection = value;
                RaisePropertyChanged(ImageCollectionPropertyName);
            }
        } 
        #endregion



        public const string BitmapImagesPropertyName = "BitmapImages";

        private ObservableCollection<BitmapImage> _bitmapImages;

        public ObservableCollection<BitmapImage> BitmapImages
        {
            get
            {
                return _bitmapImages;
            }

            set
            {
                if (_bitmapImages == value)
                {
                    return;
                }

                RaisePropertyChanging(BitmapImagesPropertyName);
                _bitmapImages = value;
                RaisePropertyChanged(BitmapImagesPropertyName);
            }
        }


        #region SelectedImage
        public const string SelectedImagePropertyName = "SelectedImage";

        private ImageModel _selectImageModel = new ImageModel();

        public ImageModel SelectedImage
        {
            get
            {
                return _selectImageModel;
            }

            set
            {
                if (_selectImageModel == value)
                {
                    return;
                }

                _selectImageModel = value;
                RaisePropertyChanged(SelectedImagePropertyName);
            }
        }
        
        #endregion

        #region Command SelectImages
        private RelayCommand _selectImages;

        public RelayCommand SelectImages
        {
            get
            {
                return _selectImages
                       ?? (_selectImages = new RelayCommand(
                           () =>
                           {
                               ReadonlyRepositoryImage repositoryImage = 
                                   new ReadonlyRepositoryImage();

                               ImageModel[] imgModels = repositoryImage.Select().ToModel().ToArray();

                               ImageCollection = new ObservableCollection<ImageModel>();

                               foreach (ImageModel imageModel in imgModels)
                               {
                                   Task<ImageModel> imageTask =
                                       new Task<ImageModel>(
                                           (imgModel) =>
                                           {
                                               ImageModel img = (ImageModel) imgModel;
                                               img.ImageBitmap =  ImageUtils.BytesToImage(img.Img);
                                               return img;
                                           },
                                           imageModel);

                                   imageTask.ContinueWith(task =>
                                   {
                                       task.Result.ImageBitmap.Freeze();
                                       
                                       ImageCollection.Add(task.Result);

                                   }, TaskScheduler.FromCurrentSynchronizationContext());

                                   imageTask.Start();
                               }
                           }));
            }
        }

        #endregion

        #region Command OpenAddPhotoForm
        private RelayCommand _openAddPhotoFormCommand;

        public RelayCommand OpenAddPhotoForm
        {
            get
            {
                return _openAddPhotoFormCommand
                    ?? (_openAddPhotoFormCommand = new RelayCommand(
                                          () =>
                                          {
                                              Messenger.Default.Send<NotificationMessageAction<bool>>(new NotificationMessageAction<bool>(MessengerMessage.OPEN_ADD_PHOTO_FORM, isAdded => AddPhotoFormClosedNotification(isAdded)));
                                          }));
            }
        }

        private void AddPhotoFormClosedNotification(bool isAdded)
        {
            if (isAdded)
            {
                SelectImages.Execute(null);
            }
        } 
        #endregion

        #region Command DeleteImage
        private RelayCommand _deleteImageCommand;

        public RelayCommand DeleteImage
        {
            get
            {
                return _deleteImageCommand
                    ?? (_deleteImageCommand = new RelayCommand(
                                          () =>
                                          {
                                              if (SelectedImage.Delete())
                                              {
                                                  ImageCollection.Remove(SelectedImage);
                                              }
                                          },
                                          () => SelectedImage != null));
            }
        } 
        #endregion

        #region Command AddManyPhotos
        private RelayCommand _addManyCommand;

        /// <summary>
        /// Gets the AddMany.
        /// </summary>
        public RelayCommand AddMany
        {
            get
            {
                return _addManyCommand
                    ?? (_addManyCommand = new RelayCommand(
                                          () =>
                                          {
                                              Messenger.Default.Send<NotificationMessageAction<List<string>>>
                                                  (new NotificationMessageAction<List<string>>(MessengerMessage.OPEN_FILE_DIALOG_FORM, 
                                                      paths => AddManyPhotoFormClosedNotification(paths) ));
                                          }));
            }
        }

        private void AddManyPhotoFormClosedNotification(List<string> paths)
        {
            RepositoryImage repositoryImage = new RepositoryImage(FNHHelper.CreateUoW());

            foreach (string path in paths)
            {
                repositoryImage.Insert(File.ReadAllBytes(path), string.Empty);

                ImageUtils.BitmapImageFromFile(path);
            }

            repositoryImage.UnitOfWork.Commit();

            if (paths.Count != 0)
            {
                SelectImages.Execute(null);
            }
        } 
        #endregion
    }
}