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

        /// <summary>
        /// The <see cref="ImageCollection" /> property's name.
        /// </summary>
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

        private RelayCommand _openAddPhotoFormCommand;

        /// <summary>
        /// Gets the OpenAddPhotoForm.
        /// </summary>
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
    }
}