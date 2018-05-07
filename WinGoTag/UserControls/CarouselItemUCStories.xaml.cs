using InstaSharper.Classes.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WinGoTag.View.StoryView;

// Il modello di elemento Controllo utente è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinGoTag.UserControls
{
    public sealed partial class CarouselItemUCStories : UserControl, INotifyPropertyChanged
    {
        public InstaStoryItem CarouselItem
        {
            get
            {
                return (InstaStoryItem)GetValue(CarouselItemProperty);
            }
            set
            {
                SetValue(CarouselItemProperty, value);
                this.DataContext = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CarouselItem"));
            }
        }

        public static readonly DependencyProperty CarouselItemProperty = DependencyProperty.Register(
         "CarouselItem",
         typeof(InstaStoryItem),
         typeof(CarouselItemUCStories),
         new PropertyMetadata(null)
        );

        public event PropertyChangedEventHandler PropertyChanged;
        //int _tapscount = 0;
        public CarouselItemUCStories()
        {
            this.InitializeComponent();
            this.DataContextChanged += CarouselItemUC_DataContextChanged;
        }
        
        private void CarouselItemUC_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            

            if (args.NewValue != null)

                if (args.NewValue.GetType() == typeof(InstaStoryItem))
                {
                    var value = DataContext as InstaStoryItem;

                    if (value.MediaType == 1)
                    {
                        CarouVideo.Visibility = Visibility.Collapsed;
                        CarouImage.Visibility = Visibility.Visible;
                        CarouImage.Source = new BitmapImage(new Uri(value.ImageList.FirstOrDefault().URI, UriKind.RelativeOrAbsolute));
                    }

                    else
                    {
                        CarouImage.Visibility = Visibility.Collapsed;
                        CarouVideo.Visibility = Visibility.Visible;
                        CarouVideo.PosterSource = new BitmapImage(new Uri(value.ImageList.FirstOrDefault().URI, UriKind.RelativeOrAbsolute));
                        CarouVideo.Source = new Uri(value.VideoList.FirstOrDefault().Url, UriKind.RelativeOrAbsolute);
                    }
                }
        }


        private void CarouVideo_GettingFocus(UIElement sender, GettingFocusEventArgs args)
        {
            CarouVideo.Play();
        }


        private void Media_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }


        private void StateVideo()
        {
            if (CarouVideo.CurrentState == MediaElementState.Buffering)
            {
                StoryViews.timer.Stop();
            }
            
            if (CarouVideo.CurrentState == MediaElementState.Paused)
            {
                StoryViews.timer.Stop();
            }

            if (CarouVideo.CurrentState == MediaElementState.Stopped)
            {
                StoryViews.timer.Stop();
            }

            if (CarouVideo.CurrentState == MediaElementState.Playing)
            {
                StoryViews.timer.Start();
            }
        }

        private void CarouImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                StoryViews.FlipViewStory.SelectedIndex++;
            }
            catch { }
        }
    }
}