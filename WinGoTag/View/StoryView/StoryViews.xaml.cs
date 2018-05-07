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
        public static DispatcherTimer timer;
        public static ProgressBar b = new ProgressBar();
        public static FlipView FlipViewStory = new FlipView();
        public StoryViews()
        {
            this.InitializeComponent();
            timer = new DispatcherTimer
            { Interval = TimeSpan.FromSeconds(1) };
            FlipViewStory = Flipviews;
            b = _BarSecond;
        }


        private void Timer_Tick(object sender, object o)
        {
            //_BarSecond.Value = _BarSecond.Value + 0.1;
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
                //timer.Stop();
        }


        private void Flipviews_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //timer.Stop();
                _BarSecond.Value = 0;
                _BarSecond.Maximum = (SecondItemList[Flipviews.SelectedIndex]);
                //timer.Start();
            }
            catch
            {

            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            

            if (e.Parameter is InstaReelFeed)
            {
                this.DataContext = ((InstaReelFeed)e.Parameter);
                var i = ((InstaReelFeed)e.Parameter);
                Flipviews.ItemsSource = i.Items;
                for (int a = 0; a < Flipviews.Items.Count; a++)
                {
                    switch (i.Items[a].MediaType)
                    {
                        case 1:
                            SecondItemList.Add(6);
                            break;

                        case 2:
                            SecondItemList.Add(i.Items[a].VideoDuration);
                            break;
                    }
                }
                //Flipviews.ItemsSource = i.Items;
            }
            else
            {
                this.DataContext = ((InstaStory)e.Parameter);
                var i = ((InstaStory)e.Parameter);
                Flipviews.ItemsSource = i.Items;
                for (int a = 0; a < Flipviews.Items.Count; a++)
                {
                    switch (i.Items[a].MediaType)
                    {
                        case InstaMediaType.Image:
                            SecondItemList.Add(6);
                            break;

                        case InstaMediaType.Video:
                            SecondItemList.Add(6);
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

            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("image");
            if (imageAnimation != null)
            { imageAnimation.TryStart(Frame); }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("imageReturn", Frame);
        }
        private void CloseStories()
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

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            Frame.Navigate(typeof(Page));
        }

        
    }
}
