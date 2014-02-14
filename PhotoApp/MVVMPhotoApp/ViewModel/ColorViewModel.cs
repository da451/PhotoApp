using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using AForge.Imaging.Filters;
using DALC;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MVVMPhotoApp.Extention;
using MVVMPhotoApp.Model;

namespace MVVMPhotoApp.ViewModel
{
    public class ColorViewModel : ViewModelBase
    {
        public ColorViewModel()
        {
            if(this.IsInDesignMode)
                return;
            PColors = new ObservableCollection<PColorModel>(FNHHelper.SelectAllPColors().ToModel());
        }

        public const string PColorsPropertyName = "PColors";

        private ObservableCollection<PColorModel> _pColors = new ObservableCollection<PColorModel>();

        public ObservableCollection<PColorModel> PColors
        {
            get
            {
                return _pColors;
            }

            set
            {
                if (_pColors == value)
                {
                    return;
                }

                RaisePropertyChanging(PColorsPropertyName);
                _pColors = value;
                RaisePropertyChanged(PColorsPropertyName);
            }
        }


        public const string SelectedColorItemPropertyName = "SelectedColorItem";

        private PColorModel _selectedColorIteModel;

        public PColorModel SelectedColorItem
        {
            get
            {
                return _selectedColorIteModel;
            }

            set
            {
                if (_selectedColorIteModel == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedColorItemPropertyName);
                _selectedColorIteModel = value;
                RaisePropertyChanged(SelectedColorItemPropertyName);
            }
        }


        private RelayCommand<PColorModel> _addItemCommand;

        public RelayCommand<PColorModel> AddItemCommand
        {
            get
            {
                return _addItemCommand
                    ?? (_addItemCommand = new RelayCommand<PColorModel>(
                                          (color) =>
                                          {
                                              FNHHelper.CreatePColor(color.Name,color.Value);
                                              SelectCommand.Execute(null);
                                          }));
            }
        }


        private RelayCommand _deleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand
                    ?? (_deleteCommand = new RelayCommand(
                                          () =>
                                          {
                                              FNHHelper.DeletePColor(SelectedColorItem.ColorID);
                                              PColors.Remove(SelectedColorItem);
                                              SelectedColorItem = null;
                                          },
                                          () => SelectedColorItem != null));
            }
        }


        private RelayCommand _selectCommand;

        public RelayCommand SelectCommand
        {
            get
            {
                return _selectCommand
                    ?? (_selectCommand = new RelayCommand(
                                          () =>
                                          {
                                              PColors =  new ObservableCollection<PColorModel>(FNHHelper.SelectAllPColors().ToModel());
                                          }));
            }
        }
    }
}