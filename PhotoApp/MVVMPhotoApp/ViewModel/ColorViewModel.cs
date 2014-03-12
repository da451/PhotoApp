using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using AForge.Imaging.Filters;
using DALC;
using DALC.Entities;
using DALC.Repository;
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

            SelectCommand.Execute(null);
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
                                              RepositoryPColor repositoryPColor =
                                                  new RepositoryPColor(FNHHelper.CreateUoW());

                                              repositoryPColor.Insert(color.Name, color.Value);

                                              repositoryPColor.UnitOfWork.Commit();

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
                               RepositoryPColor repositoryPColor =
                                   new RepositoryPColor(FNHHelper.CreateUoW());

                               repositoryPColor.Delete((PColor)SelectedColorItem);

                               repositoryPColor.UnitOfWork.Commit();

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
                                              ReadonlyRepositoryPColor readonlyRepositoryPColor =
                                                  new ReadonlyRepositoryPColor();

                                              PColors =  new ObservableCollection<PColorModel>(readonlyRepositoryPColor.Select().ToModel());
                                          }));
            }
        }
    }
}