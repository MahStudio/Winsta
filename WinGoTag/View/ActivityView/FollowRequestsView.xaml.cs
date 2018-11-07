using InstagramApiSharp.Classes.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        public FollowRequestsView() => InitializeComponent();


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(Frame.GoBack);
            myList.ItemsSource = e.Parameter;
        }

        void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
            AppCore.ModerateBack("");
        }

        void User_ItemClick(object sender, ItemClickEventArgs e) => Frame.Navigate(typeof(UserProfileView), e.ClickedItem);

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).Tag as InstaUserShort;
            AppCore.InstaApi.UserProcessor.AcceptFriendshipRequestAsync(user.Pk);
        }

        void Ignore_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).Tag as InstaUserShort;
            AppCore.InstaApi.UserProcessor.IgnoreFriendshipRequestAsync(user.Pk);
        }
    }
}
