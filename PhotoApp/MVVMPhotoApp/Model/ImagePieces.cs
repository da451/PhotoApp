using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;
using MVVMPhotoApp.Manager;
using Point = System.Drawing.Point;
using Rectangle = System.Windows.Shapes.Rectangle;
using Size = System.Drawing.Size;

namespace MVVMPhotoApp.Model
{
    public class ImagePieces : ViewModelBase
    {
        private Size _size;

        private Point _position;

        private int _index;

        private Rectangle _piece;

        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }



        public const string ColorPropertyName = "Color";

        private PColorModel _color ;

        public PColorModel Color
        {
            get
            {
                return _color;
            }

            set
            {
                if (_color == value)
                {
                    return;
                }

                RaisePropertyChanging(ColorPropertyName);
                _color = value;
                RaisePropertyChanged(ColorPropertyName);
            }
        }



        public Rectangle Piece
        {
            get { return _piece; }
            set { _piece = value; }
        }

        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; }
        }

        private BitmapImage _image;

        public ImagePieces(Size size, Point position,  BitmapImage image)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            _size = size;

            _position = position;

            _image = image;

            ImageBrush brush = new ImageBrush()
            {
                ImageSource = _image,
                Viewbox = new Rect(position.X,position.Y,size.Width,size.Height),
                ViewboxUnits = BrushMappingMode.Absolute
            };

            _piece = new Rectangle()
            {
                Height = size.Height,
                Width = size.Width,
                Fill = brush
            };

            sw.Stop();

            Console.WriteLine(string.Format("Piese cntr: {0};",sw.Elapsed.TotalSeconds));
        }

        public void FindColor()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            Color = ImageManager.Instance.FindColor(_image, _position, _size);

            sw.Stop();

            Console.WriteLine(string.Format("FindColor: {0};", sw.Elapsed.TotalSeconds));
        }
    }
}
