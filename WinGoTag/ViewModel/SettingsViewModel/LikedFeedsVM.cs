using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using WinGoTag.DataBinding;

namespace WinGoTag.ViewModel.SettingsViewModel
{
    class LikedFeedsVM :INotifyPropertyChanged
    {
        private bool _isbusy;
        private GenerateLikedFeeds<InstaMedia> _medlst;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy { get => _isbusy; set { _isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled")); } }
        public GenerateLikedFeeds<InstaMedia> MediaList
        {
            get => _medlst;
            set
            {
                _medlst = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MediaList"));
            }
        }

        public bool IsEnabled => !IsBusy;
        
        CoreDispatcher Dispatcher;
        public LikedFeedsVM()
        {
            IsBusy = true;
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            LoadPage();
        }
        
        void LoadPage()
        {
            MediaList = new GenerateLikedFeeds<InstaMedia>(100000, (count) => new InstaMedia());
            MediaList.CollectionChanged += MediaList_CollectionChanged;
        }

        private void MediaList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }
    }
}
