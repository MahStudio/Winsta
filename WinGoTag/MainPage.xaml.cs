using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Logger;
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
        public static Grid Bar { get; set; }
        public MainPage()
        {
            this.InitializeComponent();

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                try
                {
                    var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                    statusBar.BackgroundColor = Colors.White;
                    statusBar.ForegroundColor = Colors.Black;
                    statusBar.BackgroundOpacity = 1;
                }
                catch { }
            }

            MainFrame = Fr;
            Bar = InstaBar;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
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
            MainPivot.SelectedIndex = 0;
        }

        private void FindBT_Click(object sender, RoutedEventArgs e)
        {
            MainPivot.SelectedIndex = 1;
            SearchPivotItem.Content = new SearchPage();
        }

        private void AddBT_Click(object sender, RoutedEventArgs e)
        {
            MainPivot.SelectedIndex = 2;
        }

        private void LoveBT_Click(object sender, RoutedEventArgs e)
        {
            MainPivot.SelectedIndex = 3;
        }

        private void UserBT_Click(object sender, RoutedEventArgs e)
        {
            MainPivot.SelectedIndex = 4;
        }
    }
}
