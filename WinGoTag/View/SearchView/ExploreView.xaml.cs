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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WinGoTag.DataBinding;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.SearchView
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class ExploreView : Page
    {
        internal static GridViewItem itemList;
        public InstaTopLive topLive;
        public GenerateExplorePage<InstaMedia> ExplorePageItemssource;
        public ExploreView()
        {
            this.InitializeComponent();
            StoryFr.Navigate(typeof(Page));
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
            { return; }

            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("imageReturn");
            if (imageAnimation != null)
            { imageAnimation.TryStart(itemList); MainPage.Bar.Visibility = Visibility.Visible; }
        }


        private async void LoadPage()
        {
            _ProgressBar.IsIndeterminate = true;

            var strs = await AppCore.InstaApi.GetExploreFeedAsync(PaginationParameters.MaxPagesToLoad(1));

            Live.DataContext = strs.Value.StoryTray.TopLive;

            StoriesList.ItemsSource = strs.Value.StoryTray.Tray.OrderBy(x => x.SeenRankedPosition != 0);

            ListVideos.DataContext = strs.Value.Channel;
            if (ExplorePageItemssource != null)
            {
                ExplorePageItemssource.CollectionChanged -= HomePageItemssource_CollectionChanged;
            }

            ExplorePageItemssource = new GenerateExplorePage<InstaMedia>(100000, (count) =>
            {
                //return tres[count];
                return new InstaMedia();
            });

            ExplorePageItemssource.CollectionChanged += HomePageItemssource_CollectionChanged;
            
            mylist.ItemsSource = ExplorePageItemssource;

            _ProgressBar.IsIndeterminate = false;
            var sv = FindChildOfType<ScrollViewer>(mylist);
            sv.ViewChanged += Sv_ViewChanged;
        }


        private void HomePageItemssource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void Sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            VisibleItems().Play = true;
        }


        private void StoriesList_ItemClick(object sender, ItemClickEventArgs e)
        {
            SearchPage.GridAuto.Visibility = Visibility.Collapsed;
            MainPage.Bar.Visibility = Visibility.Collapsed;
            var item = ((InstaStory)e.ClickedItem);
            GridViewItem itemAnimation = (GridViewItem)StoriesList.ContainerFromItem(item);
            itemList = itemAnimation;
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("image", itemAnimation);
            StoryFr.Navigate(typeof(StoryView.StoryViews), item);
        }


        private InstaMedia VisibleItems()
        {
            List<InstaMedia> med = new List<InstaMedia>();
            var meds = mylist.ItemsSource as GenerateExplorePage<InstaSharper.Classes.Models.InstaMedia>;
            int i = 0;

            foreach (InstaMedia item in meds)
            {
                var p = (GridViewItem)mylist.ContainerFromItem(item);
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

        private void mylist_ItemClick(object sender, ItemClickEventArgs e)
        {
            SearchPage.GridAuto.Visibility = Visibility.Collapsed;
            StoryFr.Navigate(typeof(SinglePostView), e.ClickedItem);
        }

        private void Live_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StoryFr.Navigate(typeof(TopLiveVideos));
        }

        private void ListVideos_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SearchPage.GridAuto.Visibility = Visibility.Collapsed;
            StoryFr.Navigate(typeof(SinglePostView), ((e.OriginalSource as MediaElement).Tag as InstaChannel).Media );
        }
    }
}
