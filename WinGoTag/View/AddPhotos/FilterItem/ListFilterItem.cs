using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WinGoTag.View.AddPhotos.FilterItem
{
    public class ListFilterItem
    {
        public string Name { get; set; }
        public int Filter { get; set; }

        public BitmapImage Image { get; set; }
    }
}
