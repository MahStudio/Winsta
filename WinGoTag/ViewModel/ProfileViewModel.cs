using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System;
using System.ComponentModel;
using Windows.UI.Core;
using WinGoTag.DataBinding;

namespace WinGoTag.ViewModel
{
    class ProfileViewModel : INotifyPropertyChanged
    {
        bool isbusy;
        GenerateUserMedia<InstaMedia> medlst;
        GenerateUserTags<InstaMedia> medUslst;
        IResult<InstaCollections> Colst;
        InstaUserInfo info;
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsBusy
        {
            get => isbusy;
            set { isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); }
        }
        public InstaUserInfo UserInfo
        {
            get => info;
            set { info = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserInfo")); }
        }
        public GenerateUserMedia<InstaMedia> MediaList
        {
            get => medlst;
            set { medlst = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MediaList")); }
        }

        public GenerateUserTags<InstaMedia> UserTag
        {
            get => medUslst;
            set { medUslst = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserTag")); }
        }

        public IResult<InstaCollections> Collection
        {
            get => Colst;
            set { Colst = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Collection")); }
        }

        CoreDispatcher Dispatcher { get; set; }
        public ProfileViewModel()
        {
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            RunLoadPage();
        }

        async void RunLoadPage() => await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);

        async void LoadPage()
        {
            if (AppCore.InstaApi == null)
                return;
            var User = AppCore.InstaApi.GetLoggedUser();

            var user = await AppCore.InstaApi.UserProcessor.GetUserInfoByUsernameAsync(User.UserName);

            UserInfo = user.Value;

            MediaList = new GenerateUserMedia<InstaMedia>(100000, (count) => new InstaMedia()
            , User.UserName);
            // GridList = media.Value;

            UserTag = new GenerateUserTags<InstaMedia>(100000, (count) => new InstaMedia()
            , User.UserName); ;

            var collections = await AppCore.InstaApi.CollectionProcessor.GetCollectionsAsync();
            Collection = collections;
            // collections.Value.Items[0].CoverMedia
            // InstaCollections
        }
    }
}