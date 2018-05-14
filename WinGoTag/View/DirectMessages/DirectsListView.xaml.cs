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
using WinGoTag.ViewModel.DirectMessages;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.DirectMessages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DirectsListView : Page
    {
        public DirectsListView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
          
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(BackFunc);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if(e.NavigationMode == NavigationMode.Back)
                AppCore.ModerateBack("");
        }

        void BackFunc()
        {
            MainView.MainViewPivot.SelectedIndex = 1;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainView.HeaderD.Visibility = Visibility.Collapsed;
            var data = ((InstaDirectInboxThread)e.ClickedItem);
            Frame.Navigate(typeof(DirectChatView), data);
        }
    }
}
