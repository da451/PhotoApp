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

    }
}
