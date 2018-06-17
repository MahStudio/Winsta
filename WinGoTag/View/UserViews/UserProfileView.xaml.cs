using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using WinGoTag.Helpers;
using WinGoTag.ViewModel.UserViews;

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
            InitializeComponent();
            EditFr.Navigate(typeof(Page));
        }

        void HandleBiography()
        {
            try
            {
                using (var pg = new PassageHelper())
                {
                    var passages = pg.GetParagraph(UserProfileViewModel.User.Biography, HyperLinkClick);
                    txtBiography.Blocks.Clear();
                    txtBiography.Blocks.Add(passages);
                }
            }
            catch { var p = new Paragraph(); p.Inlines.Add(new Run() { Text = UserProfileViewModel.User.Biography }); txtBiography.Blocks.Add(p); }
        }

        void HyperLinkClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            if (sender == null)
                return;
            try
            {
                if (sender.Inlines.Count > 0)
                {
                    if (sender.Inlines[0] is Run run && run != null)
                    {
                        var text = run.Text;
                        text = text.ToLower();
                        run.Text.ShowInOutput();
                        if (text.StartsWith("http://") || text.StartsWith("https://") || text.StartsWith("www."))
                            run.Text.OpenUrl();
                        else
                            MainPage.Current?.PushSearch(text);
                    }
                }
            }
            catch (Exception ex) { ex.ExceptionMessage("HyperLinkClick"); }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(Frame.GoBack);
            if (e.Parameter.GetType() == typeof(BroadcastUser))
            {
                var user = e.Parameter as BroadcastUser;
                //UserProfileViewModel.User = new InstaUser(new InstaUserShort()
                //{
                //    FullName = user.FullName,
                //    IsPrivate = user.IsPrivate,
                //    IsVerified = user.IsVerified,
                //    Pk = user.Pk,
                //    ProfilePictureId = user.ProfilePicId,
                //    profile_pic_url = user.ProfilePicUrl,
                //    UserName = user.Username
                //});
                UserProfileViewModel.User = (await AppCore.InstaApi.GetUserInfoByUsernameAsync(user.Username)).Value;
                UserProfileViewModel.RunLoadPage();
            }

            if (e.Parameter.GetType() == typeof(InstaUser))
            {
                UserProfileViewModel.User = (await AppCore.InstaApi.GetUserInfoByUsernameAsync((e.Parameter as InstaUser).UserName)).Value;
                UserProfileViewModel.RunLoadPage();
            }
            if (e.Parameter.GetType() == typeof(InstaUserShort))
            {
                UserProfileViewModel.User = (await AppCore.InstaApi.GetUserInfoByUsernameAsync((e.Parameter as InstaUserShort).UserName)).Value;
                UserProfileViewModel.RunLoadPage();
            }
            HandleBiography();
        }

        void Followers_Tapped(object sender, TappedRoutedEventArgs e) => EditFr.Navigate(typeof(UserFollowersView), UserProfileViewModel.User.Username);

        void Following_Tapped(object sender, TappedRoutedEventArgs e) => EditFr.Navigate(typeof(UserFollowingsView), UserProfileViewModel.User.Username);

        void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
            AppCore.ModerateBack("");
        }

        void AdaptiveGridViewControl_ItemClick(object sender, ItemClickEventArgs e) => EditFr.Navigate(typeof(SinglePostView), e.ClickedItem as InstaMedia);

        void Followers_Click(object sender, RoutedEventArgs e) => EditFr.Navigate(typeof(UserFollowersView), UserProfileViewModel.User.Username);

        void HyperlinkButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is HyperlinkButton btn && btn != null)
                    ((string)btn.Content).OpenUrl();
            }
            catch { }
        }
    }
}