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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WinGoTag.View.UserViews;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfileView : Page
    {
        public ProfileView()
        {
            this.InitializeComponent();
            EditFr.Navigate(typeof(Page));
        }

        private void EditProfileBT_Click(object sender, RoutedEventArgs e)
        {
            var DataUser = ((Button)sender);
            var Data = ((InstaUserInfo)DataUser.DataContext);
            EditFr.Navigate(typeof(EditProfile.EditProfile), Data);
        }

        private void Followers_Tapped(object sender, TappedRoutedEventArgs e)
        {
            EditFr.Navigate(typeof(UserFollowersView), AppCore.InstaApi.GetLoggedUser().UserName);
        }

        //Following_Tapped
        private void Following_Tapped(object sender, TappedRoutedEventArgs e)
        {
            EditFr.Navigate(typeof(UserFollowingsView), AppCore.InstaApi.GetLoggedUser().UserName);
        }
    }
}
