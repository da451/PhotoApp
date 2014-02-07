using FluentNHibernate.Mapping;
using DALC.Entities;
using NHibernate.Criterion;

namespace DALC.Mapping
{
    internal class ImageMap :  ClassMap<Image>
    {
        public ImageMap()
        {
            Table("IMAGES");

            Id(x => x.ImageID).Column("IMAGE_ID").GeneratedBy.Identity();

            Map(x => x.Img).
                Column("IMG").
                CustomType("BinaryBlob").
                Length(int.MaxValue);

            Map(x => x.Thumbnail).
                Column("THUMBNAIL").
                CustomType("BinaryBlob").
                Length(int.MaxValue);
            
            Map(x => x.Name).Column("NAME");

        }
    }

}
