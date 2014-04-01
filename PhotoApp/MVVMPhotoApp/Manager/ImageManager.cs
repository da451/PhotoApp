using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using AForge.Imaging;
using DALC;
using DALC.Repository;
using MVVMPhotoApp.Model;
using MVVMPhotoApp.Utils;
using PhotoApp.Utils;
using Color = System.Windows.Media.Color;
using Image = DALC.Entities.Image;

namespace MVVMPhotoApp.Manager
{
    public class ImageManager
    {
        private readonly static ImageManager _instance = new ImageManager();

        private List<int> _imageTasks = new List<int>();

        private readonly object _synkRoot = new object();

        public static ImageManager Instance
        {
            get { return _instance; }
        }

        private ImageManager()
        {
            
        }

        public void FindColors(IEnumerable<ImageModel> images)
        {

            foreach (var imageModel in images.Where(o => !o.IsLoaded))
            {
                FindColors(imageModel);
            }
        }

        public void FindColors(ImageModel image)
        {
            Task taskFindDomainColors = new Task((model) =>
            {
                lock (_synkRoot)
                {
                    if (_imageTasks.Contains(image.ImageID))
                        return;
                }

                try
                {
                    ImageModel imageModel = (ImageModel)model;

                    lock (_synkRoot)
                    {
                        _imageTasks.Add(imageModel.ImageID);
                    }

                    Dictionary<Color, double> colorDic = AForgeUtil.ImageQuantizerByte(imageModel.Img, 3);

                    imageModel.ImageColors =
                        new ObservableCollection<PColorModel>(ColorUtil.DictionaryToKnownPColorList(colorDic));

                    RepositoryImage repositoryImageForTask =
                        new RepositoryImage(FNHHelper.CreateUoW());

                    lock (_synkRoot)
                    {

                        try
                        {
                            repositoryImageForTask.Update((Image)imageModel);

                            repositoryImageForTask.UnitOfWork.Commit();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }

                    lock (_synkRoot)
                    {
                        _imageTasks.Remove(imageModel.ImageID);
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }, image);

            taskFindDomainColors.Start();
        }

        public void SaveImages(IEnumerable<string> paths)
        {
            RepositoryImage repositoryImage = new RepositoryImage(FNHHelper.CreateUoW());

            foreach (string path in paths)
            {
                repositoryImage.Insert(File.ReadAllBytes(path), string.Empty);
            }

            repositoryImage.UnitOfWork.Commit();
        }


        public PColorModel FindColor(BitmapImage img, Point position, Size size)
        {
            Dictionary<Color, double> colorDic = AForgeUtil.ImageQuantizerByte(ImageUtils.ImageToBytes(img), position, size, 1);

            return ColorUtil.DictionaryToKnownPColorList(colorDic).First();
        }
    }
}
