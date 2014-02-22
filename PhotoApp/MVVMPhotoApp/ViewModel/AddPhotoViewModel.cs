using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DALC;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMPhotoApp.Extention;
using MVVMPhotoApp.Model;
using MVVMPhotoApp.Notifications;
using MVVMPhotoApp.Utils;
using PhotoApp.Utils;

namespace MVVMPhotoApp.ViewModel
{

    public class AddPhotoViewModel : ViewModelBase
    {
        public AddPhotoViewModel()
        {
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



        public const string ColorsPropertyName = "Colors";

        private ObservableCollection<PColorModel> _colors = new ObservableCollection<PColorModel>();

        public ObservableCollection<PColorModel> Colors
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

        private BitmapImage _oldImage;

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

       
        
        private RelayCommand _closeFormCommand;

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

        public RelayCommand SaveImage
        {
            get
            {
                return _saveImageCommand
                    ?? (_saveImageCommand = new RelayCommand(
                                          () =>
                                          {
                                              int imageID = FNHHelper.CreateImage(Image, null, Name);

                                              Task taskFindDomainColors = new Task(id =>
                                              {
                                                  ImageModel imageModel = FNHHelper.SelectImagesByID((int)id).ToImageModel();

                                                  Dictionary<Color, double> colorDic = AForgeUtil.ImageQuantizerByte(imageModel.Img, 3);

                                                  imageModel.ImageColors = new ObservableCollection<PColorModel>(ColorUtil.DictionaryToKnownPColorList(colorDic));

                                                  FNHHelper.UpdateImage((DALC.Entities.Image)imageModel);

                                              }, imageID);

                                              taskFindDomainColors.Start();

                                              Messenger.Default.Send<NotificationMessage<bool>>(new NotificationMessage<bool>(true, MessengerMessage.CLOSE_ADD_PHOTO_FORM));
                                          },
                                          () => (_imageBytes!=null && _imageBytes.Length!=0)));
            }
        }
    }
}