using InstaSharper.Classes;
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

namespace WinGoTag.View.MediaViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MediaLikersView : Page
    {
        public MediaLikersView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(Frame.GoBack);
            var likers = await AppCore.InstaApi.GetMediaLikersAsync(e.Parameter.ToString());
            myList.ItemsSource = likers.Value;
        }
        

        public void Return()
        {
            Frame.GoBack();
            AppCore.ModerateBack("");
        }
        private void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            Return();
        }

        private void User_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
