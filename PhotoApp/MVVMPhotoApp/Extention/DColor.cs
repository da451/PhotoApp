using System.Drawing;
using MColor = System.Windows.Media.Color;

namespace MVVMPhotoApp.Extention
{
    public static class DColor
    {

        public static MColor ToMediaColor(this Color color)
        {
            return MColor.FromArgb(color.A, color.R, color.G, color.B);
        }

    }
}