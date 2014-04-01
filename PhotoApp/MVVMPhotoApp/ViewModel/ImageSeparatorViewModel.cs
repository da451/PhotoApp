using System;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MVVMPhotoApp.Model;

namespace MVVMPhotoApp.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ImageSeparatorViewModel : ViewModelBase
    {
        public const string SeparatorPropertyName = "Separator";

        private Separator _separator = new Separator();

        public Separator Separator
        {
            get
            {
                return _separator;
            }

            set
            {
                if (_separator == value)
                {
                    return;
                }

                RaisePropertyChanging(SeparatorPropertyName);
                _separator = value;
                RaisePropertyChanged(SeparatorPropertyName);
            }
        }

        private RelayCommand _sliceImageCommand;

        /// <summary>
        /// Gets the SliceImage.
        /// </summary>
        public RelayCommand SliceImage
        {
            get
            {
                return _sliceImageCommand
                    ?? (_sliceImageCommand = new RelayCommand(
                                          () =>
                                          {
                                              Stopwatch sw = new Stopwatch();
                                              sw.Start();
                                              Separator.Slices(10,10);
                                              sw.Stop();
                                              Console.WriteLine(string.Format("Separetion : {0}",sw.Elapsed.TotalSeconds));

                                          }));
            }
        }

    }
}