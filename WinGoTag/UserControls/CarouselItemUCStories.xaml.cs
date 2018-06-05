using InstaSharper.Classes.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
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

                    var DPI = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().LogicalDpi;

                    var bounds = Window.Current.Bounds;
                    float scaleHeight = (float)bounds.Height / (float)value.OriginalHeight;
                    float scaleWidth = (float)bounds.Width / (float)value.OriginalWidth;

                    float scale = Math.Min(scaleHeight, scaleWidth);

                    if (value.MediaType == 1)
                    {
                        CarouVideo.Visibility = Visibility.Collapsed;
                        CarouImage.Visibility = Visibility.Visible;
                        CarouImage.Source = new BitmapImage(new Uri(value.ImageList.FirstOrDefault().URI, UriKind.RelativeOrAbsolute));

                        var ActualWidth = bounds.Width * value.OriginalWidth;
                        var ActualHeight = bounds.Height * value.OriginalHeight;

                        CarouImage.Height = (int)(value.OriginalHeight * scale);
                        CarouImage.Width = (int)(value.OriginalWidth * scale);
                    }

                    else
                    {
                        CarouImage.Visibility = Visibility.Collapsed;
                        CarouVideo.Visibility = Visibility.Visible;
                        CarouVideo.PosterSource = new BitmapImage(new Uri(value.ImageList.FirstOrDefault().URI, UriKind.RelativeOrAbsolute));
                        CarouVideo.Source = new Uri(value.VideoList.FirstOrDefault().Url, UriKind.RelativeOrAbsolute);

                        var ActualWidth = bounds.Width * value.OriginalWidth;
                        var ActualHeight = bounds.Height * value.OriginalHeight;

                        CarouVideo.Height = (int)(value.OriginalHeight * scale);
                        CarouVideo.Width = (int)(value.OriginalWidth * scale);
                    }

                    if (value.ReelMentions.Count > 0)
                    {
                        foreach (var Mention in value.ReelMentions)
                        {
                            var ActualX = bounds.Width * Mention.X * scale;
                            var ActualY = bounds.Height * Mention.Y * scale;

                            var ActualWidth = bounds.Width * Mention.Width;
                            var ActualHeight = bounds.Height * Mention.Height;

                            Rectangle rectangle = new Rectangle()
                            {
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                Margin = new Thickness(ActualX, ActualY, 0, 0),
                                Width = ActualWidth,
                                Height = ActualHeight,
                                Fill = new SolidColorBrush(Colors.DarkRed)
                            };
                            MainGr.Children.Add(rectangle);
                            //Mention.X * Window.Current.Bounds.
                        }
                    }
                    if (value.StoryCTA != null)
                    {
                        SeeMoreGrid.Visibility = Visibility.Visible;
                        if (value.LinkText != null)
                            AdLinkText.Text = value.LinkText;
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

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            var value = DataContext as InstaStoryItem;
            var uri = value.StoryCTA.FirstOrDefault().Links.FirstOrDefault().WebUri;
            await Launcher.LaunchUriAsync(new Uri(uri, UriKind.RelativeOrAbsolute));
        }
    }
}