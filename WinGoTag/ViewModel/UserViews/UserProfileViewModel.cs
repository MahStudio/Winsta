using InstaSharper.Classes.Models;
using System;
using System.ComponentModel;
using Windows.UI.Core;
using WinGoTag.DataBinding;

namespace WinGoTag.ViewModel.UserViews
{
    class UserProfileViewModel : INotifyPropertyChanged
    {
        bool isbusy;
        string flwcon;
        GenerateUserMedia<InstaMedia> medlst;
        GenerateUserTags<InstaMedia> medUslst;
        InstaUserInfo info;
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsBusy
        {
            get => isbusy;
            set { isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); }
        }
        public string FollowBTNContent
        {
            get => flwcon;
            set
            {
                flwcon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FollowBTNContent"));
            }
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

        CoreDispatcher Dispatcher { get; set; }

        public InstaUserInfo User { get; set; }

        public AppCommand FollowBTNCmd { get; set; }
        public UserProfileViewModel()
        {
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            FollowBTNCmd = AppCommand.GetInstance();
            FollowBTNCmd.ExecuteFunc = Follow;
        }

        async void Follow(object obj)
        {
            switch (FollowBTNContent)
            {
                case "Follow":
                    var flwstat = await AppCore.InstaApi.FollowUserAsync(User.Pk);
                    if (flwstat.Value.OutgoingRequest)
                        FollowBTNContent = "Requested";


                    if (flwstat.Value.Following)
                        FollowBTNContent = "Unfollow";


                    break;
                case "Unfollow":
                    var flw = await AppCore.InstaApi.UnFollowUserAsync(User.Pk);
                    if (!flw.Value.Following)
                        FollowBTNContent = "Follow";


                    break;
                case "Message":
                    throw new Exception("Not implemented yet");
                    break;
                case "Requested":
                    throw new Exception("Not implemented yet");
                    break;
                default:
                    break;
            }
        }

        public async void RunLoadPage() => await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);

        async void LoadPage()
        {
            var user = await AppCore.InstaApi.GetUserInfoByUsernameAsync(User.Username);
            var status = await AppCore.InstaApi.GetFriendshipStatusAsync(user.Value.Pk);
            if (!status.Value.Following)
                FollowBTNContent = "Follow";


            if (status.Value.OutgoingRequest)
                FollowBTNContent = "Requested";


            if (status.Value.IncomingRequest)
            {
                // Allow Accept request or Deny
            }

            if (status.Value.Following)
                FollowBTNContent = "Unfollow";
            // if(user.Value.HasChaining && status.Value.Following)
            // {
            //    FollowBTNContent = "Message";
            // }

            UserInfo = user.Value;

            MediaList = new GenerateUserMedia<InstaMedia>(100000, (count) => new InstaMedia()
            , User.Username);
            // GridList = media.Value;
            MediaList.CollectionChanged += MediaList_CollectionChanged;
            UserTag = new GenerateUserTags<InstaMedia>(100000, (count) =>
            {
                // return tres[count];
                return new InstaMedia();
            }, User.Username);
            // UserTag = mediaUserTag.Value;
        }

        void MediaList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // if ((e.NewItems[0] as InstaMedia).Pk == "App")
            // {
            //    //Private and Not Followed
            //    MediaList = null;
            //    FollowBTNContent = "Follow";
            // }
            // else
            // {
            //    if (UserInfo.HasChaining)
            //        FollowBTNContent = "Message";
            //    else
            //        FollowBTNContent = "Unfollow";
            // }
        }
    }
}