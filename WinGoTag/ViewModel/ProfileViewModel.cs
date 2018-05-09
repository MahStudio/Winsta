using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.ComponentModel;
using Windows.UI.Core;
using WinGoTag.DataBinding;

namespace WinGoTag.ViewModel
{
    class ProfileViewModel : INotifyPropertyChanged
    {
        private bool _isbusy;
        private GenerateUserMedia<InstaMedia> _medlst;
        private GenerateUserTags<InstaMedia> _medUslst;
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
        public GenerateUserMedia<InstaMedia> MediaList
        {
            get { return _medlst; }
            set { _medlst = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MediaList")); }
        }
        
        public GenerateUserTags<InstaMedia> UserTag
        {
            get { return _medUslst; }
            set { _medUslst = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserTag")); }
        }

        CoreDispatcher Dispatcher { get; set; }
        public ProfileViewModel()
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
            var User = AppCore.InstaApi.GetLoggedUser();
            var user = await AppCore.InstaApi.GetUserInfoByUsernameAsync(User.UserName);
            UserInfo = user.Value;

            MediaList = new GenerateUserMedia<InstaMedia>(100000, (count) =>
            {
                //return tres[count];
                return new InstaMedia();
            }, User.UserName);
            //GridList = media.Value;

            UserTag = new GenerateUserTags<InstaMedia>(100000, (count) =>
            {
                //return tres[count];
                return new InstaMedia();
            }, User.UserName); ;
        }
    }
}
