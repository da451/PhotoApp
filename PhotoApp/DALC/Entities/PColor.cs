using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALC.Entities
{
    public class PColor
    {
        public PColor()
        {
            Images = new List<Image>();
        }

        public PColor( string value, string name)
            : this()
        {
            Value = value;
            Name = name;
        }

        public PColor(int colorID, string value, string name)
            : this()
        {
            ColorID = colorID;
            Value = value;
            Name = name;
        }

        public virtual int ColorID { get; set; }

        public virtual string Name { get; set; }

        public virtual string Value { get; set; }

        public virtual IList<Image> Images { get; set; }
    }
}
