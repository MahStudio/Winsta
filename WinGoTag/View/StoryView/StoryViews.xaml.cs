using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WinGoTag.View.SearchView;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.StoryView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StoryViews : Page
    {
        public static event EventHandler<bool> StartStopTimerEvent;
        List<double> SecondItemList = new List<double>();
        public static DispatcherTimer timer;
        public static ProgressBar b = new ProgressBar();
        public static FlipView FlipViewStory = new FlipView();
        public StoryViews()
        {
            InitializeComponent();
            timer = new DispatcherTimer
            { Interval = TimeSpan.FromSeconds(1) };
            FlipViewStory = Flipviews;
            b = _BarSecond;
            StartStopTimerEvent += StoryViews_StartStopTimerEvent;
        }

        void StoryViews_StartStopTimerEvent(object sender, bool e)
        {
            if (e)
                timer.Stop();
            else
                timer.Start();
        }

        public static void PauseStartTimer(bool Pause) => StartStopTimerEvent?.Invoke(null, Pause);

        void Timer_Tick(object sender, object o)
        {
            // _BarSecond.Value = _BarSecond.Value + 0.1;
            _BarSecond.Value++;
            if (_BarSecond.Value == SecondItemList[Flipviews.SelectedIndex])
            {
                try
                {
                    Flipviews.SelectedIndex++;
                }
                catch
                {
                    CloseStories();
                }
            }
        }

        void Flipviews_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // timer.Stop();
                _BarSecond.Value = 0;
                _BarSecond.Maximum = (SecondItemList[Flipviews.SelectedIndex]);

                var items = Flipviews.ItemsSource as List<InstaStoryItem>;
                var selecteditem = items[Flipviews.SelectedIndex];
                //var p = items.Where(x => x.Play == true);
                //foreach (var item in p) item.Play = false;

                //selecteditem.Play = true;
                // timer.Start();
            }
            catch
            {
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(CloseStories);
            if (e.Parameter is InstaReelFeed)
            {
                DataContext = ((InstaReelFeed)e.Parameter);
                AnimationEnter();
                var i = ((InstaReelFeed)e.Parameter);
                InstaReelFeed story = null;
                if ((e.Parameter as InstaReelFeed).Items.Count != 0)
                    story = e.Parameter as InstaReelFeed;
                else
                    story = (await AppCore.InstaApi.StoryProcessor.GetUserStoryFeedAsync(i.User.Pk)).Value;

                //foreach (var item in story.Items) item.Play = false;


                Flipviews.ItemsSource = story.Items;
                for (int a = 0; a < Flipviews.Items.Count; a++)
                    switch (story.Items[a].MediaType)
                    {
                        case 1:
                            SecondItemList.Add(6);
                            break;

                        case 2:
                            SecondItemList.Add(story.Items[a].VideoDuration);
                            break;
                    }

                //story.Items[0].Play = true;
                // Flipviews.ItemsSource = i.Items;

                // var strs = await AppCore.InstaApi.LiveProcessor.SeenBroadcastAsync(i.Id.ToString(), i.HasBestiesMedia.ToString());
            }
            else
            {
                DataContext = ((InstaStory)e.Parameter);
                AnimationEnter();
                var i = ((InstaStory)e.Parameter);
                try
                {
                    var story = await AppCore.InstaApi.StoryProcessor.GetUserStoryFeedAsync(i.User.Pk);
                    //foreach (var item in story.Value.Items) item.Play = false;


                    Flipviews.ItemsSource = story.Value.Items;
                    for (int a = 0; a < Flipviews.Items.Count; a++)
                    {
                        switch (story.Value.Items[a].MediaType)
                        {
                            case 1:
                                SecondItemList.Add(6);
                                break;

                            case 2:
                                SecondItemList.Add(story.Value.Items[a].VideoDuration);
                                break;
                        }
                    }

                    //story.Value.Items[0].Play = true;
                }
                catch
                {
                    var story = await AppCore.InstaApi.StoryProcessor.GetUserStoryFeedAsync(i.Owner.Pk);
                    // foreach (var item in story.Items) { item.Play = false; }
                    Flipviews.ItemsSource = story.Value.Items;
                    for (int a = 0; a < Flipviews.Items.Count; a++)
                        switch (story.Value.Items[a].MediaType)
                        {
                            case 1:
                                SecondItemList.Add(6);
                                break;

                            case 2:
                                SecondItemList.Add(story.Value.Items[a].VideoDuration);
                                break;
                        }
                }
            }

            if (Flipviews.Items.Count > 0)
            {
                _BarSecond.Maximum = (SecondItemList[0]);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("imageReturn", Frame);
            try
            {
                SearchPage.GridAuto.Visibility = Visibility.Visible;
            }
            catch { }
        }

        void CloseStories()
        {
            try
            {
                timer.Stop();
                Frame.Navigate(typeof(Page));
            }
            catch
            {
            }
        }

        public void AnimationEnter()
        {
            var imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("image");
            if (imageAnimation != null)
            { imageAnimation.TryStart(Frame); }
        }

        void BackBT_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            Frame.Navigate(typeof(Page));
        }
    }
}