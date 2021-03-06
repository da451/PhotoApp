﻿using System.Collections.Generic;
using System.Linq;
using DALC.Interfaces;
using FluentNHibernate.Utils;
using NHibernate.Mapping;

namespace DALC.Entities
{
    public class Image : IEntity
    {
        public Image()
        {
            Colors = new List<PColor>();
        }

        public Image(int imageID, byte[] img, byte[] thumbnail, string name, IList<PColor> colors)
            : this()
        {
            ImageID = imageID;
            Img = img;
            Thumbnail = thumbnail;
            Name = name;
            Colors = colors;
        }

        public Image(byte[] img, byte[] thumbnail, string name, IList<PColor> colors)
            : this()
        {
            Img = img;
            Thumbnail = thumbnail;
            Name = name;
            Colors = colors;
        }

        public virtual int ImageID { get; set; }

        public virtual byte[] Img { get; set; }

        public virtual byte[] Thumbnail { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<PColor> Colors { get; set; }

        public virtual int PrimaryKey
        {
            get
            {
                return ImageID;
            }
        }

        public virtual void AddColor(PColor color)
        {
            if(!Colors.Any(o => o.ColorID == color.ColorID))
                Colors.Add(color);
        }

    }


}
