using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DALC.Entities;
using MVVMPhotoApp.Model;


namespace MVVMPhotoApp.Extention
{
    public static class CollectionOfEntityExtensions
    {
        //public static ObservableCollection<ImageModel> ToModel(this IEnumerable<Image> images)
        //{
        //    ObservableCollection<ImageModel> res = new ObservableCollection<ImageModel>();
            
        //    if (images != null)
        //    {
        //        var imageModels = images.Select(o => (ImageModel)o);

        //        res = new ObservableCollection<ImageModel>(imageModels);
        //    }

        //    return res;
        //}

        public static List<Image> ToEntity(this IEnumerable<ImageModel> imageModels)
        {
            List<Image> res = new List<Image>();

            if (imageModels != null)
            {
                var images = imageModels.Select(o => (Image)o);

                res.AddRange(images);
            }

            return res;
        }

        public static List<PColor> ToEntity(this IEnumerable<PColorModel> colorModels)
        {
            List<PColor> res = new List<PColor>();

            if (colorModels != null)
            {
                var images = colorModels.Select(o => (PColor)o);

                res.AddRange(images);
            }

            return res;
        }

    }
}
