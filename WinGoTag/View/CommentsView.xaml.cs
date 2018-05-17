using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinGoTag.DataBinding;
using WinGoTag.View.UserViews;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CommentsView : Page
    {
        string MediaID = "";
        public GenerateComments<InstaComment> PageItemssource;
        public CommentsView()
        {
            this.InitializeComponent();
        }
        private void MessageTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                SendTextMessage();
        }
        async void SendTextMessage()
        {
            var res = await AppCore.InstaApi.CommentMediaAsync(MediaID, MessageTextBox.Text);
            if(!res.Succeeded)
            {
                await new MessageDialog(res.Info.Message).ShowAsync();
                return;
            }
            else
            {
                PageItemssource.Add(res.Value);
                MessageTextBox.Text = "";
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(Frame.GoBack);
            MediaID = e.Parameter.ToString();
            if (PageItemssource != null)
            {
                PageItemssource.CollectionChanged -= PageItemssource_CollectionChanged;
            }

            PageItemssource = new GenerateComments<InstaComment>(100000, (count) =>
            {
                //return tres[count];
                return new InstaComment();
            }, e.Parameter.ToString());

            PageItemssource.CollectionChanged += PageItemssource_CollectionChanged;
            mylist.ItemsSource = PageItemssource;
        }

        private void PageItemssource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
            AppCore.ModerateBack("");
        }

        private void Username_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserProfileView), (sender as HyperlinkButton).Tag);
        }
    }
}
