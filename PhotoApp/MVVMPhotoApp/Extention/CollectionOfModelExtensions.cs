using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DALC.Entities;
using MVVMPhotoApp.Model;


namespace MVVMPhotoApp.Extention
{
    public static class CollectionOfModelExtensions
    {
        public static ObservableCollection<ImageModel> ToModel(this IEnumerable<Image> images)
        {
            ObservableCollection<ImageModel> res = new ObservableCollection<ImageModel>();
            
            if (images != null && images.Count()!=0)
            {
                var imageModels = images.Select(o => o.ToImageModel()).ToList();

                res = new ObservableCollection<ImageModel>(imageModels);
            }

            return res;
        }

        public static ObservableCollection<PColorModel> ToModel(this IEnumerable<PColor> pcolors)
        {
            ObservableCollection<PColorModel> res = new ObservableCollection<PColorModel>();

            if (pcolors != null && pcolors.Count() != 0)
            {
                var colorModels = pcolors.Select(o => new PColorModel(o.ColorID, o.Value, o.Name)).ToList();

                res = new ObservableCollection<PColorModel>(colorModels);
            }

            return res;
        }
    }
}
