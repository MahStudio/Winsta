using InstaSharper.Classes.Models;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinGoTag.View;

// Il modello di elemento Controllo utente è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinGoTag.UserControls.DirectMessageUCs
{
    public sealed partial class DirectMessageItemUC : UserControl, INotifyPropertyChanged
    {
        public InstaDirectInboxItem InboxItem
        {
            get
            {
                return (InstaDirectInboxItem)GetValue(MediaProperty);
            }
            set
            {
                SetValue(MediaProperty, value);
                this.DataContext = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InboxItem"));
            }
        }
        public static readonly DependencyProperty MediaProperty = DependencyProperty.Register(
         "InboxItem",
         typeof(InstaDirectInboxItem),
         typeof(DirectMessageItemUC),
         new PropertyMetadata(null)
        );

        public event PropertyChangedEventHandler PropertyChanged;

        public DirectMessageItemUC()
        {
            this.InitializeComponent();
        }

        private async void Likes_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, LikeDislikeRunner);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var data = (this.DataContext as InstaDirectInboxItem);
            MainPage.MainFrame.Navigate(typeof(SinglePostView), data.MediaShare);
        }

        private async void LikeDislikeRunner()
        {
            if (!InboxItem.MediaShare.HasLiked)
            {
                if ((await AppCore.InstaApi.LikeMediaAsync(InboxItem.MediaShare.InstaIdentifier)).Value)
                {
                    InboxItem.MediaShare.HasLiked = true;
                    InboxItem.MediaShare.LikesCount += 1;
                }
            }
            else
            {
                if ((await AppCore.InstaApi.UnLikeMediaAsync(InboxItem.MediaShare.InstaIdentifier)).Value)
                {
                    InboxItem.MediaShare.HasLiked = false;
                    InboxItem.MediaShare.LikesCount -= 1;
                }
            }
        }

        private void LikesImage_Click(object sender, RoutedEventArgs e)
        {
            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, LikeDislikeRunnerMedia);
        }

        //private async void LikeDislikeRunnerMedia()
        //{
        //    if (!InboxItem.MediaShare.HasLiked)
        //    {
        //        if ((await AppCore.InstaApi.LikeMediaAsync(InboxItem.MediaShare.InstaIdentifier)).Value)
        //        {
        //            InboxItem.MediaShare.HasLiked = true;
        //            InboxItem.MediaShare.LikesCount += 1;
        //        }
        //    }
        //    else
        //    {
        //        if ((await AppCore.InstaApi.UnLikeMediaAsync(InboxItem.MediaShare.InstaIdentifier)).Value)
        //        {
        //            InboxItem.MediaShare.HasLiked = false;
        //            InboxItem.MediaShare.LikesCount -= 1;
        //        }
        //    }
        //}
    }
}
