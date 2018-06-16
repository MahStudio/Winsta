using System.Collections.ObjectModel;

namespace WinGoTag.View.AddPhotos.FilterItem
{
    public class AddFilterItem
    {
        public ObservableCollection<ListFilterItem> ListFilterItems =
             new ObservableCollection<ListFilterItem>();

        string[] Names = { "Test", "Test-2", "Test-3", "Test-4", "Test-5" };

        int[] Filters = { 0, 1, 2, 3, 4, 5 };

        public AddFilterItem()
        {
            for (int i = 0; i < Names.Length; i++)
                ListFilterItems.Add(new ListFilterItem()
                {
                    Name = Names[i],
                    Filter = Filters[i],
                    //Image = EditPhotoVideoView.bitmapImage
                });
        }
    }
}