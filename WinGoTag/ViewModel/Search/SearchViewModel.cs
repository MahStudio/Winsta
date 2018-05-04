using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using WinGoTag.DataBinding;

namespace WinGoTag.ViewModel.Search
{
    class SearchViewModel : INotifyPropertyChanged
    {
        private bool _isbusy;

        private List<InstaStory> _Traylst;

        private InstaChannel _Videolst;

        private InstaMediaList _medlst;

        public GenerateHomePage<InstaMedia> HomePageItemssource;

        private InstaTagFeed _Tag;

        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsBusy
        {
            get { return _isbusy; }
            set { _isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); }
        }

        //public InstaUserInfo UserInfo   ListVideos
        //{
        //    get { return _info; }
        //    set { _info = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserInfo")); }
        //}

        public InstaChannel ListVideos
        {
            get { return _Videolst; }
            set { _Videolst = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ListVideos")); }
        }

        public InstaMediaList mylist
        {
            get { return _medlst; }
            set { _medlst = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("mylist")); }
        }

        public List<InstaStory> StoriesList
        {
            get { return _Traylst; }
            set { _Traylst = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StoriesList")); }
        }

        CoreDispatcher Dispatcher { get; set; }
        public SearchViewModel()
        {
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            RunLoadPage();
        }

        async void RunLoadPage()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);
        }

        async void LoadPage()
        {
            //var Tag = await AppCore.InstaApi.GetTagFeedAsync("cosedalover", InstaSharper.Classes.PaginationParameters.MaxPagesToLoad(1));

            var strs = await AppCore.InstaApi.GetExploreFeedAsync(PaginationParameters.MaxPagesToLoad(1));
            StoriesList = strs.Value.StoryTray.Tray;
            mylist = strs.Value.Medias;
            ListVideos = strs.Value.Channel;
            
        }
    }
}
