using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace WinGoTag.ViewModel.UserViews
{
    class UserProfileViewModel : INotifyPropertyChanged
    {
        private bool _isbusy;
        private InstaMediaList _medlst;
        private InstaMediaList _medUslst;
        private InstaUserInfo _info;
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsBusy
        {
            get { return _isbusy; }
            set { _isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); }
        }
        public InstaUserInfo UserInfo
        {
            get { return _info; }
            set { _info = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserInfo")); }
        }
        public InstaMediaList MediaList
        {
            get { return _medlst; }
            set { _medlst = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MediaList")); }
        }

        public InstaMediaList GridList
        {
            get { return _medlst; }
            set { _medlst = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GridList")); }
        }

        public InstaMediaList UserTag
        {
            get { return _medUslst; }
            set { _medUslst = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserTag")); }
        }

        CoreDispatcher Dispatcher { get; set; }

        public UserProfileViewModel()
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
            //var user = await AppCore.InstaApi.GetUserInfoByUsernameAsync(Username);
            //UserInfo = user.Value;

            //var media = await AppCore.InstaApi.GetUserMediaAsync(Username, PaginationParameters.MaxPagesToLoad(1));
            //MediaList = media.Value;
            //GridList = media.Value;

            //var mediaUserTag = await AppCore.InstaApi.GetUserTagsAsync(Username, PaginationParameters.MaxPagesToLoad(1));
            //UserTag = mediaUserTag.Value;
        }
    }
}
