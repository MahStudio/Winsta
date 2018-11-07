using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Classes.ResponseWrappers;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
            get => (string)GetValue(RecentActivityProperty);
            set
            {
                SetValue(RecentActivityProperty, value);
                DataContext = value;
            }
        }
        public static readonly DependencyProperty RecentActivityProperty = DependencyProperty.Register(
         "RecentActivity",
         typeof(string),
         typeof(RecentActivityView),
         new PropertyMetadata(null)
        );
        List<InstaUserShortResponse> PendingUsers;
        public RecentActivity<InstaRecentActivityFeed> RecentActivityItemssource;
        public RecentActivityView()
        {
            InitializeComponent();
            EditFr.Navigate(typeof(Page));
            OnNavigatedTo();
            // RecentActivity
            // var RecentActivity = await AppCore.InstaApi.GetRecentActivityAsync(PaginationParameters.MaxPagesToLoad(1));
        }

        public async void OnNavigatedTo()
        {
            PendingUsers = (await AppCore.InstaApi.UserProcessor.GetPendingFriendRequestsAsync()).Value.Users.ToList();
            if (PendingUsers == null || PendingUsers.Count == 0)
                FollowReqItem.Visibility = Visibility.Collapsed;

            if (RecentActivityItemssource != null)
                RecentActivityItemssource.CollectionChanged -= PageItemssource_CollectionChanged;

            RecentActivityItemssource = new RecentActivity<InstaRecentActivityFeed>(100000, (count) => new InstaRecentActivityFeed());
            // var RecentActivity = await AppCore.InstaApi.GetRecentActivityAsync(PaginationParameters.MaxPagesToLoad(1));

            RecentActivityItemssource.CollectionChanged += PageItemssource_CollectionChanged;
            ListYouActivity.ItemsSource = RecentActivityItemssource;
        }

        void PageItemssource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) { }

        void FollowReqItem_Tapped(object sender, TappedRoutedEventArgs e) =>
            EditFr.Navigate(typeof(FollowRequestsView), PendingUsers);


        void ItemActivityTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            // var Data = ((Grid)sender);
            // var Context = ((InstaRecentActivityFeed)Data.DataContext);
        }
    }
}