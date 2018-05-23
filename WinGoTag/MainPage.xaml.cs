using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Logger;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinGoTag.View;
using WinGoTag.View.ActivityView;
using WinGoTag.View.AddPhotos;
using WinGoTag.View.DirectMessages;
using WinGoTag.View.SearchView;
using WinGoTag.View.SignInSignUp;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WinGoTag
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static Frame MainFrame { get; set; }
        public static DropShadowPanel Bar { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            FrameConnect.Navigate(typeof(Page));
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                try
                {
                    var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                    SolidColorBrush statuscolor = Root.Background as SolidColorBrush;
                    statusBar.BackgroundColor = statuscolor.Color;
                    statusBar.ForegroundColor = Colors.Black;
                    statusBar.BackgroundOpacity = 1;
                }
                catch { }
            }
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
            MainFrame = FrameConnect;
            Bar = InstaBar;
        }

        private void MainPage_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            e.Handled = true;
            AppCore.ModerateBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            IconBarHome.Glyph = "\ueA8A";
            Fr.Navigated += Fr_Navigated;
            if (AppCore.InstaApi == null)
                Fr.Navigate(typeof(LoginView));
            else
                Fr.Navigate(typeof(MainView));
        }
        private void Fr_Navigated(object sender, NavigationEventArgs e)
        {
            if (Fr.Content is LoginView)
            {
                InstaBar.Visibility = Visibility.Collapsed;
            }
            if (Fr.Content is DirectsListView)
            {
                InstaBar.Visibility = Visibility.Collapsed;
            }

            if (Fr.Content is MainView)
            {
                InstaBar.Visibility = Visibility.Visible;
                if (AppCore.InstaApi != null)
                { ProfilePivotItem.Content = new ProfileView(); }
            }

        }

        private void HomeBT_Click(object sender, RoutedEventArgs e)
        {

            //Select &#xEA8A;
            //Unselect &#xE10F;
            IconLight();
            IconBarHome.Glyph = "\ueA8A";
            MainPivot.SelectedIndex = 0;
        }

        private void FindBT_Click(object sender, RoutedEventArgs e)
        {
            //Unselect &#xE71E;
            IconLight();
            IconBarFind.FontWeight = Windows.UI.Text.FontWeights.Bold;
            MainPivot.SelectedIndex = 1;
            SearchPivotItem.Content = new SearchPage();
        }

        private void AddBT_Click(object sender, RoutedEventArgs e)
        {
            //IconLight();
            MainPage.Bar.Visibility = Visibility.Collapsed;
            FrameConnect.Navigate(typeof(PhotoGalleryView));
        }

        private void LoveBT_Click(object sender, RoutedEventArgs e)
        {
            //Select &#xEB52;
            //Unselect &#xEB51;
            IconLight();
            IconBarLover.Glyph = "\ueB52";
            MainPivot.SelectedIndex = 3;
            ActivityPivotItem.Content = new RecentActivityView();
        }

        private void UserBT_Click(object sender, RoutedEventArgs e)
        {
            //Select &#xEA8C;
            //Unselect &#xE2AF;
            IconLight();
            IconBarUser.Glyph = "\ueA8C";
            MainPivot.SelectedIndex = 4;
        }

        public void IconBold()
        {
            
        }

        public void IconLight()
        {
            IconBarHome.Glyph = "\ue10F";

            IconBarFind.FontWeight = Windows.UI.Text.FontWeights.Normal;

            IconBarLover.Glyph = "\ueB51";

            IconBarUser.Glyph = "\ue2AF";
        }
    }
}
