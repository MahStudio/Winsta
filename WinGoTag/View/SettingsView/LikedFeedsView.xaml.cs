using InstaSharper.Classes.Models;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.SettingsView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LikedFeedsView : Page
    {
        public LikedFeedsView()
        {
            this.InitializeComponent();
            EditFr.Navigate(typeof(Page));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(Frame.GoBack);
        }

        void Return()
        {
            Frame.GoBack();
            AppCore.ModerateBack("");
        }

        private void AdaptiveGridViewControl_ItemClick(object sender, ItemClickEventArgs e) => EditFr.Navigate(typeof(SinglePostView), e.ClickedItem as InstaMedia);

        private void ToBackBT_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)=> Return();
    }
}
