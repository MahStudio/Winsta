using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using WinGoTag.DataBinding;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.ActivityView
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class RecentActivityView : Page
    {
        public string RecentActivity
        {
            get
            {
                return (string)GetValue(RecentActivityProperty);
            }
            set
            {
                SetValue(RecentActivityProperty, value);
                this.DataContext = value;
            }
        }
        public static readonly DependencyProperty RecentActivityProperty = DependencyProperty.Register(
         "RecentActivity",
         typeof(string),
         typeof(RecentActivityView),
         new PropertyMetadata(null)
        );
        List<InstaUserShort> PendingUsers;
        public RecentActivity<InstaRecentActivityFeed> RecentActivityItemssource;
        public RecentActivityView()
        {
            this.InitializeComponent();
            EditFr.Navigate(typeof(Page));
            OnNavigatedTo();
            //RecentActivity
            //var RecentActivity = await AppCore.InstaApi.GetRecentActivityAsync(PaginationParameters.MaxPagesToLoad(1));
        }

        public async void OnNavigatedTo()
        {
            PendingUsers = (await AppCore.InstaApi.GetPendingFriendRequests()).Value.Users.ToList();
            if (PendingUsers == null || PendingUsers.Count == 0)
            {
                FollowReqItem.Visibility = Visibility.Collapsed;
            }
            if (RecentActivityItemssource != null)
            {
                RecentActivityItemssource.CollectionChanged -= PageItemssource_CollectionChanged;
            }
            RecentActivityItemssource = new RecentActivity<InstaRecentActivityFeed>(100000, (count) =>
            {
                return new InstaRecentActivityFeed();
            });
            //var RecentActivity = await AppCore.InstaApi.GetRecentActivityAsync(PaginationParameters.MaxPagesToLoad(1));

            RecentActivityItemssource.CollectionChanged += PageItemssource_CollectionChanged;
            ListYouActivity.ItemsSource = RecentActivityItemssource;
        }


        private void PageItemssource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void FollowReqItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            EditFr.Navigate(typeof(FollowRequestsView), PendingUsers);
        }

        private void ItemActivityTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            //var Data = ((Grid)sender);
            //var Context = ((InstaRecentActivityFeed)Data.DataContext);
            
        }
    }
}
