using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using DALC;
using DALC.Mapping;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMPhotoApp.Notifications;
using MVVMPhotoApp.Utils;

namespace MVVMPhotoApp.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class TestViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the TestViewModel class.
        /// </summary>
        public TestViewModel()
        {
        }

        /// <summary>
        /// The <see cref="Colors" /> property's name.
        /// </summary>
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

        /// <summary>
        /// The <see cref="Image" /> property's name.
        /// </summary>
        public const string ImagePropertyName = "Image";

        private BitmapImage _image = new BitmapImage();

        public BitmapImage Image
        {
            get
            {
                return _image;
            }

            set
            {
                if (_image == value)
                {
                    return;
                }

                RaisePropertyChanging(ImagePropertyName);
                _image = value;
                RaisePropertyChanged(ImagePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="NewImage" /> property's name.
        /// </summary>
        public const string NewImagePropertyName = "NewImage";

        private BitmapImage _newImage = new BitmapImage();

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
            Image = ImageUtils.BitmapImageFromFile(path);
        }

        private RelayCommand _convertCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand Convert
        {
            get
            {
                return _convertCommand
                    ?? (_convertCommand = new RelayCommand(
                                          () =>
                                          {
                                              NewImage = AForgeUtil.ImageQuantizer(Image);
                                              IList<Color> colors = AForgeUtil.GetImagePalette(Image);
                                              Colors = new ObservableCollection<Color>(colors);
                                          }));
            }
        }
    }
}