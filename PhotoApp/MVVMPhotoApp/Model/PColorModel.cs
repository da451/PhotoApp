using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace MVVMPhotoApp.Model
{
    public class PColorModel : ViewModelBase
    {
        public PColorModel(int colorId, string value, string name)
        {
            _colorID = colorId;
            _value = value;
            _name = name;
        }

        public PColorModel()
        {
        }
        public PColorModel(string value, string name)
        {
            _value = value;
            _name = name;
        }


        public const string ColorIDPropertyName = "ColorID";

        private int _colorID;

        public int ColorID
        {
            get
            {
                return _colorID;
            }

            set
            {
                if (_colorID == value)
                {
                    return;
                }

                RaisePropertyChanging(ColorIDPropertyName);
                _colorID = value;
                RaisePropertyChanged(ColorIDPropertyName);
            }
        }



        public const string ValuePropertyName = "Value";

        private string _value;

        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (_value == value)
                {
                    return;
                }

                RaisePropertyChanging(ValuePropertyName);
                _value = value;
                RaisePropertyChanged(ValuePropertyName);
            }
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
    }
}
