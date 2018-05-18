using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinGoTag.View.AddPhotos.FilterItem
{
    public class AddFilterItem
    {
        public ObservableCollection<ListFilterItem> ListFilterItems =
             new ObservableCollection<ListFilterItem>();


        private string[] Names = { "Test", "Test-2", "Test-3", "Test-4", "Test-5" };

        private int[] Filters = { 0, 1, 2, 3, 4, 5 };

        public AddFilterItem()
        {
            for (int i = 0; i < Names.Length; i++)
            {
                ListFilterItems.Add(new ListFilterItem()
                {
                   Name = Names[i],
                   Filter = Filters[i],
                   //Image = EditPhotoVideoView.bitmapImage
                });
            }
        }
    }
}
