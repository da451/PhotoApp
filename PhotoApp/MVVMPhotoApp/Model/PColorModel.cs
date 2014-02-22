using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALC.Entities;
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

        public PColorModel(string value, string name, double persent)
        {
            Value = value;
            Name = name;
            Percent = persent;
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



        /// <summary>
        /// The <see cref="Percent" /> property's name.
        /// </summary>
        public const string PercentPropertyName = "Percent";

        private double _percent;

        /// <summary>
        /// Sets and gets the Percent property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double Percent
        {
            get
            {
                return _percent;
            }

            set
            {
                if (_percent == value)
                {
                    return;
                }

                RaisePropertyChanging(PercentPropertyName);
                _percent = value;
                RaisePropertyChanged(PercentPropertyName);
            }
        }


        public static explicit operator PColor(PColorModel pcolor)
        {

            return new PColor(pcolor.ColorID, pcolor.Value, pcolor.Name);
        }
    }
}
