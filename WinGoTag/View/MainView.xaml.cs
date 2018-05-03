using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WinGoTag.DataBinding;
using WinGoTag.View.DirectMessages;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page
    {
        bool _loadDirect = true;
        public GenerateHomePage<InstaMedia> HomePageItemssource;
        internal static dynamic itemList;
        public MainView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            StoryFr.Navigated += StoryFr_Navigated;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);
        }


        private void StoryFr_Navigated(object sender, NavigationEventArgs e)
        {
            if (StoryFr.Content is StoryView.StoryViews)
            {
                return;
            }

            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("imageReturn");
            if (imageAnimation != null)
            { imageAnimation.TryStart(itemList); }
        }

        private async void LoadPage()
        {
            _ProgressBar.IsIndeterminate = true;
            var res = await AppCore.InstaApi.GetUserTimelineFeedAsync(InstaSharper.Classes.PaginationParameters.MaxPagesToLoad(1));
            if (res.Info.Message == "login_required")
            {
                AppCore.InstaApi = null;
                AppCore.SaveUserInfo(null, null, false);
                MainPage.MainFrame.GoBack();
                return;
            }
            var strs = await AppCore.InstaApi.GetStoryFeedAsync();
            StoriesList.ItemsSource = strs.Value.Items.OrderBy(x => x.Seen != 0);
            if (HomePageItemssource != null)
            {
                HomePageItemssource.CollectionChanged -= HomePageItemssource_CollectionChanged;
            }
            HomePageItemssource = new GenerateHomePage<InstaMedia>(100000, (count) =>
            {
                //return tres[count];
                return new InstaMedia();
            });
            HomePageItemssource.CollectionChanged += HomePageItemssource_CollectionChanged;
            //MediasCVS.Source = HomePageItemssource;
            mylist.ItemsSource = HomePageItemssource;
            //await HomePageItemssource.LoadMoreItemsAsync(20);
            //mylist.ItemsSource = res.Value.Medias;
            //mylist.ItemsSource = res.Value.Medias;
            _ProgressBar.IsIndeterminate = false;
            var sv = FindChildOfType<ScrollViewer>(mylist);
            sv.ViewChanged += Sv_ViewChanged; 

            //await Task.Delay(2000);

            //DirectFr.Navigate(typeof(DirectsListView));
            //MainPage.MainFrame.Navigate(typeof(DirectsListView));
        }

        private void HomePageItemssource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void Sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            VisibleItems().Play = true;
        }

        private void DirectBT_Click(object sender, RoutedEventArgs e)
        {
            PivotView.SelectedIndex = 1;
            //Scroller.ChangeView(null, 0, null);
        }

        private void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            PivotView.SelectedIndex = 0;
        }

        private void StoriesList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListViewItem itemAnimation = (ListViewItem)StoriesList.ContainerFromItem(e.ClickedItem);
            itemList = itemAnimation;
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("image", itemAnimation);
            var item = ((InstaReelFeed)e.ClickedItem);
            StoryFr.Navigate(typeof(StoryView.StoryViews), item);
            //item.Items[].VideoList[].Url
        }

        private void PivotView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (PivotView.SelectedIndex)
            {
                case 0:

                    break;

                case 1:
                    if (_loadDirect == true)
                    {
                        DirectFr.Navigate(typeof(DirectsListView));
                        _loadDirect = false;
                    }
                    break;
            }
        }
        
        private InstaMedia VisibleItems()
        {
            List<InstaMedia> med = new List<InstaMedia>();
            var meds = mylist.ItemsSource as GenerateHomePage<InstaSharper.Classes.Models.InstaMedia>;
            int i = 0;
            foreach (InstaMedia item in meds)
            {
                var p = (ListViewItem)mylist.ContainerFromItem(item);
                if (IsVisibileToUser(p, mylist))
                {
                    med.Add(item);
                }
                i++;
            }
            
            return med.FirstOrDefault();
            //try
            //{
            //    var sv = FindChildOfType<ScrollViewer>(mylist);
            //    var p = mylist.TransformToVisual(sv).TransformPoint(new Point(0, 0));
            //    var topitem = sv.VerticalOffset;
                
            //    var visibleitemscount = sv.ViewportHeight;
            //    if (p.X > topitem && p.X < topitem + visibleitemscount)
            //    {

            //    }
            //    return (mylist.ItemsSource as List<InstaMedia>)[Convert.ToInt32((topitem / visibleitemscount))];
            //}
            //catch { return null; }
        }
        private bool IsVisibileToUser(FrameworkElement element, FrameworkElement container)
        {
            if (element == null || container == null)
                return false;

            Rect elementBounds = element.TransformToVisual(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            Rect containerBounds = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);

            return (elementBounds.Left < containerBounds.Right && elementBounds.Right > containerBounds.Left);
        }
        public static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }
    }
}
