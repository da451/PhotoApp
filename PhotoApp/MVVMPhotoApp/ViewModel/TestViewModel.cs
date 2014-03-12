using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DALC;
using DALC.Mapping;
using DALC.Repository;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMPhotoApp.Extention;
using MVVMPhotoApp.Model;
using MVVMPhotoApp.Notifications;
using MVVMPhotoApp.Utils;
using PhotoApp.Utils;
using ColorConverter = System.Windows.Media.ColorConverter;

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
        public TestViewModel()
        {
            ReadonlyRepositoryPColor repositoryPColor =
                new ReadonlyRepositoryPColor();

            _baseColors = repositoryPColor.Select().ToModel().ToList();

        }

        private readonly object syncRoot = new object();

        private List<PColorModel> _baseColors;

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
            Image = ImageUtils.BitmapImageFromFile(path);
        }


        private RelayCommand _convertCommand;

        public RelayCommand Convert
        {
            get
            {
                return _convertCommand
                    ?? (_convertCommand = new RelayCommand(
                                          () =>
                                          {
                                              Task<byte[]> imgQuanTask = new Task<byte[]>(
                                                  (img) =>
                                                  {
                                                      byte[] byteImage;
                                                      Dictionary<Color,int> imageColors =
                                                          AForgeUtil.ImageQuantizerByte((BitmapImage)img, 3, out  byteImage);
                                                      return byteImage;
                                                  }, Image);
                                              imgQuanTask.ContinueWith(task =>
                                              {
                                                  try
                                                  {
                                                      NewImageByte = task.Result;
                                                      
                                                  }
                                                  catch (Exception ex)
                                                  {
                                                      throw ex;
                                                  }
                                                  finally
                                                  {
                                                  }
                                              }, TaskScheduler.FromCurrentSynchronizationContext());
                                              imgQuanTask.Start();

                                              Task<IList<Color>> GetImagePaletteTask = new Task<IList<Color>>((img)=>  AForgeUtil.GetImagePalette((BitmapImage) img),Image);
                                              GetImagePaletteTask.ContinueWith(task =>
                                              {
                                                  TupleColors = new ObservableCollection<Tuple<Color, Color, Color>>();

                                                  Colors = new ObservableCollection<Color>(task.Result);

                                                  foreach (Color color in Colors)
                                                  {
                                                      TupleColors.Add(StringToColor(color));
                                                  }
                                              }, TaskScheduler.FromCurrentSynchronizationContext());
                                              GetImagePaletteTask.Start();
                                          }));
            }
        }

        private Tuple<Color, Color, Color> StringToColor(Color color)
        {
            IList<PColorModel> colors = ColorUtil.CompareColors(_baseColors, new PColorModel(color.ToString(), ""), 3);

            return new Tuple<Color, Color, Color>(
                PColorModelToColor(colors[0]), 
                PColorModelToColor(colors[1]), 
                PColorModelToColor(colors[2])
                );
        }

        private Color PColorModelToColor(PColorModel color)
        {
            var colorObj = ColorConverter.ConvertFromString(color.Value);
            if (colorObj != null)
            {
                Color c = (Color)ColorConverter.ConvertFromString(colorObj.ToString());
                return c;
            }
            return new Color();
        }

        public const string TupleColorsPropertyName = "TupleColors";

        private ObservableCollection<Tuple<Color, Color, Color>> _tupleColors;

        public ObservableCollection<Tuple<Color, Color, Color>> TupleColors
        {
            get
            {
                return _tupleColors;
            }

            set
            {
                if (_tupleColors == value)
                {
                    return;
                }

            //lock (syncRoot)
                {

                    RaisePropertyChanging(TupleColorsPropertyName);
                    _tupleColors = value;
                    RaisePropertyChanged(TupleColorsPropertyName);
                }
            }
        }

        public const string NewImageBytePropertyName = "NewImageByte";

        private byte[] _newImageByte = new byte[0];

        public byte[] NewImageByte
        {
            get
            {
                return _newImageByte;
            }

            set
            {
                if (_newImageByte == value)
                {
                    return;
                }

                RaisePropertyChanging(NewImageBytePropertyName);
                _newImageByte = value;
                RaisePropertyChanged(NewImageBytePropertyName);
            }
        }

    }
}


