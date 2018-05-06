using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.StoryView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StoryViews : Page
    {
        List<double> SecondItemList = new List<double>();

        private DispatcherTimer _timer;
        
        private DateTime _lastChange;

        DispatcherTimer _value = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

        double v = 1;

        public StoryViews()
        {
            this.InitializeComponent();
        }


        private void ChangeImage(object sender, object o)
        {
            //Get the number of items in the flip view
            var totalItems = Flipviews.Items.Count;
            if(totalItems == 1) { CloseStories(); return; }
            var index = Flipviews.Items.Count-1;
        
            if (Flipviews.SelectedIndex == index) { CloseStories(); return; }
            //Figure out the new item's index (the current index plus one, if the next item would be out of range, go back to zero)
            var newItemIndex = (Flipviews.SelectedIndex + 1) % totalItems;
            //Set the displayed item's index on the flip view
            Flipviews.SelectedIndex = newItemIndex;

            v = 0;

            _timer = new DispatcherTimer
            { Interval = TimeSpan.FromSeconds(SecondItemList[Flipviews.SelectedIndex]) };
            _BarSecond.Value = v;
            _BarSecond.Maximum = (SecondItemList[Flipviews.SelectedIndex]);
        }


        private void ValueBar(object sender, object o)
        {
            v++;
            _BarSecond.Value = v;
        }

        private void DisplayedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            //Show the time deltas...
            var currentTime = DateTime.Now;

            if (_lastChange != default(DateTime))
            {
                //  TimeDelta.Text = (currentTime - _lastChange).ToString();
            }

            _lastChange = currentTime;
            //Since the page is configured before the timer is, check to make sure that we've actually got a timer
            if (!ReferenceEquals(_timer, null))
            {
                _timer.Stop();
                _timer.Start();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //var test = AppCore.InstaApi.GetStoryFeedAsync(((InstaReelFeed)e.Parameter).User.Pk);

            if (e.Parameter is InstaReelFeed)
            {
                this.DataContext = ((InstaReelFeed)e.Parameter);
                var i = ((InstaReelFeed)e.Parameter);
                Flipviews.ItemsSource = i.Items;
            }

            if (e.Parameter is InstaStory)
            {
                this.DataContext = ((InstaStory)e.Parameter);
                var i = ((InstaStory)e.Parameter);
                Flipviews.ItemsSource = i.Items;
            }


            //for (int a = 0; a < i.Items.Count; a++)
            //{
            //    switch (i.Items[a].MediaType)
            //    {
            //        case 1:
            //            SecondItemList.Add(6);
            //            break;

            //        case 2:
            //            SecondItemList.Add(i.Items[a].VideoDuration);
            //            break;
            //    }
            //}

            //if(i.Items.Count > 0)
            //{
            //    _timer = new DispatcherTimer
            //    { Interval = TimeSpan.FromSeconds(SecondItemList[0]) };

            //    _BarSecond.Maximum = (SecondItemList[0]);
            //    Flipviews.ItemsSource = i.Items;

            //    _timer.Tick += ChangeImage;
            //    _value.Tick += ValueBar;
            //    //Start the timer
            //    //_timer.Start();
            //    //_value.Start();
            //}

            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("image");
            if (imageAnimation != null)
            { imageAnimation.TryStart(Frame); }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            CloseStories();
        }
        private void CloseStories()
        {
            try
            {
                _timer.Stop();
                _value.Stop();
                _timer = null;
                _value = null;
            }
            catch
            {
                _timer = null;
                _value = null;
            }
           
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("imageReturn", Frame);
            
        }

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page));
            
        }
    }
}
