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
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WinGoTag.Helpers;
using WinGoTag.View.SettingsView;
using WinGoTag.View.UserViews;
using WinGoTag.ViewModel;

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
            DataContextChanged += ProfileView_DataContextChanged;
        }

        private void ProfileView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            try
            {
                if (args.NewValue.GetType() == typeof(ProfileViewModel))
                {
                    var context = args.NewValue as ProfileViewModel;
                    if (context != null)
                    {
                        using (var pg = new PassageHelper())
                        {
                            var passages = pg.GetParagraph(context.UserInfo.Biography, HyperLinkClick);
                            txtBiography.Blocks.Clear();
                            txtBiography.Blocks.Add(passages);
                        }
                    }
                }
            } catch { }
        }
        private void HyperLinkClick(Hyperlink sender, HyperlinkClickEventArgs args)
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
                        {
                            MainPage.Current?.PushSearch(text);
                        }
                    }
                }
            }
            catch (Exception ex) { ex.ExceptionMessage("CaptionHyperLinkClick"); }
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

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            EditFr.Navigate(typeof(MainSettingsView));
        }

        private void PivotUserView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (PivotUserView.SelectedIndex)
            {
                case 0:
                    
                    break;

                case 1:

                    break;

                case 2:

                    break;

                case 3:

                    break;
            }
        }

        private void AdaptiveGridViewControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            EditFr.Navigate(typeof(SinglePostView), e.ClickedItem);
        }

        private void HyperlinkButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is HyperlinkButton btn && btn != null)
                {
                    ((string)btn.Content).OpenUrl();
                }
            }
            catch { }
        }
    }
}
