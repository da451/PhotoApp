using FluentNHibernate.Utils;

namespace DALC.Entities
{
    public class Image
    {
        public Image()
        {
        }

        public Image(int imageID, byte[] img, byte[] thumbnail, string name)
            : this()
        {
            ImageID = imageID;
            Img = img;
            Thumbnail = thumbnail;
            Name = name;
        }

        public Image(byte[] img, byte[] thumbnail, string name)
            : this()
        {
            Img = img;
            Thumbnail = thumbnail;
            Name = name;
        }

        public virtual int ImageID { get; set; }

        public virtual byte[] Img { get; set; }

        public virtual byte[] Thumbnail { get; set; }

        public virtual string Name { get; set; }

    }


}
