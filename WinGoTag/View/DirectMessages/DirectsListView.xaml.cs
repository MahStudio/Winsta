using InstagramApiSharp.Classes.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.DirectMessages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DirectsListView : Page
    {
        public DirectsListView() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(BackFunc);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
                AppCore.ModerateBack("");
        }

        void BackFunc() => MainView.MainViewPivot.SelectedIndex = 1;

        void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainView.HeaderD.Visibility = Visibility.Collapsed;
            var data = ((InstaDirectInboxThread)e.ClickedItem);
            Frame.Navigate(typeof(DirectChatView), data);
        }
    }
}