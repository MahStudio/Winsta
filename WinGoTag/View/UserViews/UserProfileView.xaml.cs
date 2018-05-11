using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.UserViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserProfileView : Page
    {
        public UserProfileView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(Frame.GoBack);
            if(e.Parameter.GetType() == typeof(BroadcastUser))
            {
                var user = e.Parameter as BroadcastUser;
                UserProfileViewModel.User = new InstaUser(new InstaUserShort()
                {
                    FullName = user.FullName, IsPrivate = user.IsPrivate, IsVerified = user.IsVerified, Pk = user.Pk,
                    ProfilePictureId = user.ProfilePicId, profile_pic_url = user.ProfilePicUrl, UserName = user.Username
                });
            }
            if (e.Parameter.GetType() == typeof(InstaUser))
                UserProfileViewModel.User = (e.Parameter as InstaUser);
            if (e.Parameter.GetType() == typeof(InstaUserShort))
                UserProfileViewModel.User = new InstaUser(e.Parameter as InstaUserShort);
        }

        private void Followers_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //EditFr.Navigate(typeof(UserFollowersView), UserProfileViewModel.User.UserName);
        }

        private void Following_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //EditFr.Navigate(typeof(UserFollowingsView), UserProfileViewModel.User.UserName);
        }
        
        private void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
            AppCore.ModerateBack("");
        }
    }
}
