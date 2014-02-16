using System.Collections.ObjectModel;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DALC;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMPhotoApp.Model;
using MVVMPhotoApp.Notifications;

namespace MVVMPhotoApp.ViewModel
{

    public class AddPhotoViewModel : ViewModelBase
    {
        public AddPhotoViewModel()
        {
        }

        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

        private string _name;

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
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
        /// The <see cref="Image" /> property's name.
        /// </summary>
        public const string ImagePropertyName = "Image";

        private byte[] _imageBytes;

        public byte[] Image
        {
            get
            {
                return _imageBytes;
            }

            set
            {
                if (_imageBytes == value)
                {
                    return;
                }

                RaisePropertyChanging(ImagePropertyName);
                _imageBytes = value;
                RaisePropertyChanged(ImagePropertyName);
            }
        }

        private RelayCommand _closeFormCommand;

        /// <summary>
        /// Gets the CloseForm.
        /// </summary>
        public RelayCommand CloseForm
        {
            get
            {
                return _closeFormCommand
                    ?? (_closeFormCommand = new RelayCommand(
                                          () =>
                                          {
                                              Messenger.Default.Send<NotificationMessage<bool>>(new NotificationMessage<bool>(false, MessengerMessage.CLOSE_ADD_PHOTO_FORM));
                                          }));
            }
        }

        private RelayCommand _openFileDialogCommand;

        /// <summary>
        /// Gets the OpenFileDialog.
        /// </summary>
        public RelayCommand OpenFileDialog
        {
            get
            {
                return _openFileDialogCommand
                    ?? (_openFileDialogCommand = new RelayCommand(
                                          () =>
                                          {
                                              var message =
                                                  new NotificationMessageAction<string>(
                                                      MessengerMessage.OPEN_FILE_DIALOG_FORM, GetImageSourse);
                                              Messenger.Default.Send(message);
                                          }));
            }
        }

        private void GetImageSourse(string path)
        {
            Image = File.ReadAllBytes(path);
        }


        private RelayCommand _saveImageCommand;

        /// <summary>
        /// Gets the SaveImage.
        /// </summary>
        public RelayCommand SaveImage
        {
            get
            {
                return _saveImageCommand
                    ?? (_saveImageCommand = new RelayCommand(
                                          () =>
                                          {
                                              FNHHelper.CreateImage(Image, null, Name);
                                              Messenger.Default.Send<NotificationMessage<bool>>(new NotificationMessage<bool>(true, MessengerMessage.CLOSE_ADD_PHOTO_FORM));
                                          },
                                          () => (_imageBytes!=null && _imageBytes.Length!=0)));
            }
        }




        public const string ColorsPropertyName = "Colors";

        private ObservableCollection<Color> _colors = new ObservableCollection<Color>();

        public ObservableCollection<Color> Colors
        {
            get
            {
                return _colors;
            }

            set
            {
                if (_colors == value)
                {
                    return;
                }

                RaisePropertyChanging(ColorsPropertyName);
                _colors = value;
                RaisePropertyChanged(ColorsPropertyName);
            }
        }


        public const string OldImagePropertyName = "OldImage";

        private BitmapImage _oldImage ;

        public BitmapImage OldImage
        {
            get
            {
                return _oldImage;
            }

            set
            {
                if (_oldImage == value)
                {
                    return;
                }

                RaisePropertyChanging(OldImagePropertyName);
                _oldImage = value;
                RaisePropertyChanged(OldImagePropertyName);
            }
        }
      


        public const string NewImagePropertyName = "NewImage";

        private BitmapImage _newImage;

        public BitmapImage NewImage
        {
            get
            {
                return _newImage;
            }

            set
            {
                if (_newImage == value)
                {
                    return;
                }

                RaisePropertyChanging(NewImagePropertyName);
                _newImage = value;
                RaisePropertyChanged(NewImagePropertyName);
            }
        }
    }
}