using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
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
        internal static GridViewItem itemList;
        public static DropShadowPanel HeaderD;
        public static Pivot MainViewPivot;
        public static Frame MainFrame;
        public MainView()
        {
            this.InitializeComponent();
            //StoryFr.Navigate(typeof(Page));
            HeaderD = HeaderDirect;
            MainFrame = StoryFr;
            MainViewPivot = PivotView;
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
            { imageAnimation.TryStart(itemList); MainPage.Bar.Visibility = Visibility.Visible; }
        }


        private async void LoadPage()
        {
            _ProgressBar.IsIndeterminate = true;
            var p = PaginationParameters.MaxPagesToLoad(1);
            var res = await AppCore.InstaApi.GetDirectInboxAsync(p);
            p.NextId = res.Value.Inbox.OldestCursor;
            var res2 = await AppCore.InstaApi.GetDirectInboxAsync(p);
            if (res.Info.Message == "login_required")
            {
                //AppCore.InstaApi = null;
                AppCore.SaveUserInfo(null, null, false);
                MainPage.MainFrame.GoBack();
                return;
            }


            //TEST
            var User = AppCore.InstaApi.GetLoggedUser();
            var user = await AppCore.InstaApi.GetUserInfoByUsernameAsync(User.UserName);
            var items = await AppCore.InstaApi.GetUserStoryFeedAsync(user.Value.Pk);
            InstaUserShort You = new InstaUserShort() { UserName = "You", ProfilePicture = user.Value.ProfilePicUrl, Pk = user.Value.Pk };
            InstaReelFeed MyReel = new InstaReelFeed();

            MyReel.User = You;
            //MyReel.Seen = 0;
            MyReel.Items = items.Value.Items;
            //var m = (await AppCore.InstaApi.GetUserTimelineFeedAsync(p));
            //var first = m.Value.Medias.LastOrDefault();
            //var s = await AppCore.InstaApi.GetMediaCommentsAsync(first.InstaIdentifier.ToString(), p);

            //var sss = "";

            //          var ss = await AppCore.InstaApi.GetMediaLikersAsync(first.InstaIdentifier.ToString());

            //var sssds = "";
            var strs = await AppCore.InstaApi.GetStoryFeedAsync();

            if (strs.Value.Items.Exists(x => x.User.Pk == user.Value.Pk))
            {
                //strs.Value.Items.OrderBy(x => x.Seen != 0);
                var MyRemove = strs.Value.Items.FindIndex(x => x.User.Pk == user.Value.Pk);
                MyReel.Seen = strs.Value.Items[MyRemove].Seen;
                MyReel.LatestReelMedia = strs.Value.Items[MyRemove].LatestReelMedia;
                strs.Value.Items.RemoveAt(MyRemove);
            }
            
            //

            strs.Value.Items.OrderBy(x => x.Seen_ranked_position != 0);
            strs.Value.Items.Insert(0, MyReel);

            StoriesList.ItemsSource = strs.Value.Items;
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



        private async void Sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            await Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, delegate
            {
                ScrollViewer sv = sender as ScrollViewer;
                GeneralTransform gt = sv.TransformToVisual(this);
                Point p = gt.TransformPoint(new Point(0, 0));
                List<UIElement> list = new List<UIElement>(VisualTreeHelper.FindElementsInHostCoordinates(p, sv));
                ListViewItem item = list.OfType<ListViewItem>().FirstOrDefault();
                if (item != null)
                {
                    int index = mylist.IndexFromContainer(item);
                    //Debug.WriteLine("Visible item at top of list is " + index);
                    var meds = mylist.ItemsSource as GenerateHomePage<InstaMedia>;
                    foreach (InstaMedia med in meds)
                    {
                        if (med.Play)
                            med.Play = false;
                    }
                    ((InstaMedia)meds[index]).Play = true;
                }
            });
        }

        private void DirectBT_Click(object sender, RoutedEventArgs e)
        {
            PivotView.SelectedIndex = 2;
        }

        private void CameraBT_Click(object sender, RoutedEventArgs e)
        {
            PivotView.SelectedIndex = 0;
        }

        private void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            PivotView.SelectedIndex = 1;
        }

        private void BackFromCameraBT_Click(object sender, RoutedEventArgs e)
        {
            PivotView.SelectedIndex = 1;
            AppCore.ModerateBack("");
        }

        private void StoriesList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = ((InstaReelFeed)e.ClickedItem);
            if(item.User.UserName is "You")
            {
              if(item.Items.Count is 0)
                {
                    PivotView.SelectedIndex = 0;
                    return;
                }
            }

            GridViewItem itemAnimation = (GridViewItem)StoriesList.ContainerFromItem(item);
            
            itemList = itemAnimation;
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("image", itemAnimation);
            StoryFr.Navigate(typeof(StoryView.StoryViews), item);
            MainPage.Bar.Visibility = Visibility.Collapsed;
        }

        private void PivotView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (PivotView.SelectedIndex)
            {
                case 0:
                    MainPage.Bar.Visibility = Visibility.Collapsed;
                    CameraFr.Navigate(typeof(CameraView.CameraView));
                    break;

                case 1:
                    MainPage.Bar.Visibility = Visibility.Visible;
                    CameraFr.Navigate(typeof(Page));
                    CameraFr.BackStack.Clear();
                    break;

                case 2:
                    MainPage.Bar.Visibility = Visibility.Collapsed;
                    if (_loadDirect == true)
                    {
                        DirectFr.Navigate(typeof(DirectsListView));
                        _loadDirect = false;
                    }
                    break;
            }
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

        private async void RefreshBT_Click(object sender, RoutedEventArgs e)
        {
            FindChildOfType<ScrollViewer>(mylist).ChangeView(0,0,null);
            FindChildOfType<ScrollViewer>(StoriesList).ChangeView(0, 0, null);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);
        }

        private void CinemaBT_Click(object sender, RoutedEventArgs e)
        {
            BlurBackground.Visibility = Visibility.Visible;
            PlayCinema();
        }

        private async void PlayCinema()
        {
            var en = HomePageItemssource.ToList();
            foreach (InstaMedia item in en)
            {
                if (item.MediaType == InstaMediaType.Image)
                {
                    CinemaPlayer.Source = null;
                    CinemaPlayer.PosterSource = new BitmapImage(new Uri(item.Images[0].URI, UriKind.RelativeOrAbsolute));
                    await Task.Delay(6000);
                }
                if(item.MediaType == InstaMediaType.Video)
                {
                    CinemaPlayer.PosterSource = new BitmapImage(new Uri(item.Images[0].URI, UriKind.RelativeOrAbsolute));
                    await Task.Delay(3000);
                    CinemaPlayer.Source = new Uri(item.Videos[0].Url, UriKind.RelativeOrAbsolute);
                    await Task.Delay(Convert.ToInt32(CinemaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds));
                }
                if(item.MediaType == InstaMediaType.Carousel)
                {
                    foreach (var sub in item.Carousel)
                    {
                        if (item.MediaType == InstaMediaType.Image)
                        {
                            CinemaPlayer.Source = null;// new Uri(item.Images[0].URI, UriKind.RelativeOrAbsolute);
                            CinemaPlayer.PosterSource = new BitmapImage(new Uri(item.Images[0].URI, UriKind.RelativeOrAbsolute));
                            await Task.Delay(6000);
                        }
                        if (item.MediaType == InstaMediaType.Video)
                        {
                            CinemaPlayer.PosterSource = new BitmapImage(new Uri(item.Images[0].URI, UriKind.RelativeOrAbsolute));
                            await Task.Delay(3000);
                            CinemaPlayer.Source = new Uri(item.Videos[0].Url, UriKind.RelativeOrAbsolute);
                            await Task.Delay(60000);
                        }
                        
                    }
                }
            }
        }
    }
}