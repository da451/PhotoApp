using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DALC;
using DALC.Repository;
using GalaSoft.MvvmLight;
using MVVMPhotoApp.Extention;
using MVVMPhotoApp.Utils;

namespace MVVMPhotoApp.Model
{
    public class Separator : ViewModelBase
    {

        public const string ImagePropertyName = "Image";

        private BitmapImage _image;

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



        public const string PiecesPropertyName = "Pieces";

        private ObservableCollection<ImagePieces> _pieces = new ObservableCollection<ImagePieces>();

        public ObservableCollection<ImagePieces> Pieces
        {
            get
            {
                return _pieces;
            }

            set
            {
                if (_pieces == value)
                {
                    return;
                }

                RaisePropertyChanging(PiecesPropertyName);
                _pieces = value;
                RaisePropertyChanged(PiecesPropertyName);
            }
        }

        public Separator()
        {
            RepositoryImage repository = new RepositoryImage(FNHHelper.CreateUoW());

            ImageModel img = repository.Get(157).ToImageModel();

            Image = ImageUtils.BytesToImage(img.Img);
        }

        public void Slices(int verticalCount, int horizontalCount)
        {
            int verticalStep = (int) (this.Image.Height/verticalCount);

            int horizontalStep = (int)(this.Image.Width / horizontalCount);

            var taskList = new List<Task>();

            for (int v = 0; v < verticalCount; v++)
            {
                for (int h = 0; h < horizontalCount; h++)
                {
                    

                   ImagePieces p = new ImagePieces(new Size(horizontalStep, verticalStep),new Point(h*horizontalStep, v*verticalStep), Image);

                    _pieces.Add(p);

                    Task separatorTask = new Task(
                        (imagePiece) =>
                        {
                            try
                            {
                                ImagePieces piece = (ImagePieces) imagePiece;

                                piece.FindColor();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        },p);

                    taskList.Add(separatorTask);

                    separatorTask.Start();

                }
            }

            Task.WaitAll(taskList.ToArray());
        }
    }
}
