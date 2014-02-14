using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALC.Entities;
using FluentNHibernate.Mapping;

namespace DALC.Mappings
{
    public class PColorMap : ClassMap<PColor>
    {
        public PColorMap()
        {
            Table("PCOLORS");

            Id(x => x.ColorID).Column("COLOR_ID").GeneratedBy.Identity();

            Map(x => x.Name).Column("NAME");

            Map(x => x.Value).Column("VALUE");
        }
    }
}
