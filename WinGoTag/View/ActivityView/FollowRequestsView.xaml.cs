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
using WinGoTag.View.UserViews;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.ActivityView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FollowRequestsView : Page
    {
        public FollowRequestsView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AppCore.ModerateBack(Frame.GoBack);
            myList.ItemsSource = e.Parameter;
        }

        private void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void User_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(UserProfileView), e.ClickedItem);
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).Tag as InstaUserShort;
            AppCore.InstaApi.AcceptFriendshipRequest(user.Pk);
        }

        private void Ignore_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).Tag as InstaUserShort;
            AppCore.InstaApi.IgnoreFriendshipRequest(user.Pk);
        }
    }
}
