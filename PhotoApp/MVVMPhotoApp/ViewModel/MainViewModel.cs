using System.Collections.ObjectModel;
using System.Linq;
using DALC;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMPhotoApp.Extention;
using MVVMPhotoApp.Model;
using MVVMPhotoApp.Notifications;

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

        #region SelectedImage
        public const string SelectedImagePropertyName = "SelectedImage";

        private ImageModel _selectImageModel = null;

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

                RaisePropertyChanging(SelectedImagePropertyName);
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
                                              ImageCollection = FNHHelper.SelectAllImages().ToModel();
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
                                              if (FNHHelper.DeleteImage(SelectedImage.ImageID))
                                              {
                                                  SelectImages.Execute(null);
                                              }
                                          },
                                          () => SelectedImage != null));
            }
        } 
        #endregion
    }
}