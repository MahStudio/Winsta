using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinGoTag.View.DirectMessages;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);
        }

        private async void LoadPage()
        {
            var res = await AppCore.InstaApi.GetUserTimelineFeedAsync(InstaSharper.Classes.PaginationParameters.MaxPagesToLoad(5));
            if (res.Info.Message == "login_required")
            {
                AppCore.InstaApi = null;
                AppCore.SaveUserInfo(null, null, false);
                MainPage.MainFrame.GoBack();
                return;
            }
            var strs = await AppCore.InstaApi.GetStoryFeedAsync();
            StoriesList.ItemsSource = strs.Value.Items.OrderBy(x => x.Seen != 0);
            mylist.ItemsSource = res.Value.Medias;
            //await Task.Delay(2000);
            //MainPage.MainFrame.Navigate(typeof(DirectsListView));
        }
    }
}
