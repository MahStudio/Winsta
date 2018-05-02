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

// Il modello di elemento Controllo utente è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinGoTag.UserControls
{
    public sealed partial class CarouselItemUCStories : UserControl, INotifyPropertyChanged
    {
        public InstaCarouselItem CarouselItem
        {
            get
            {
                return (InstaCarouselItem)GetValue(CarouselItemProperty);
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
         typeof(InstaCarouselItem),
         typeof(CarouselItemUCStories),
         new PropertyMetadata(null)
        );

        public event PropertyChangedEventHandler PropertyChanged;
        int _tapscount = 0;
        public CarouselItemUCStories()
        {
            this.InitializeComponent();
            this.DataContextChanged += CarouselItemUC_DataContextChanged;
        }
        
        private void CarouselItemUC_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue != null)

                if (args.NewValue.GetType() == typeof(InstaCarouselItem))

                {

                    var value = DataContext as InstaCarouselItem;

                    if (value.MediaType == InstaMediaType.Image)

                    {

                        CarouVideo.Visibility = Visibility.Collapsed;

                        CarouImage.Visibility = Visibility.Visible;

                        CarouImage.Source = new BitmapImage(new Uri(value.Images.FirstOrDefault().URI, UriKind.RelativeOrAbsolute));

                    }

                    else

                    {

                        CarouImage.Visibility = Visibility.Collapsed;

                        CarouVideo.Visibility = Visibility.Visible;

                        CarouVideo.PosterSource = new BitmapImage(new Uri(value.Images.FirstOrDefault().URI, UriKind.RelativeOrAbsolute));

                        CarouVideo.Source = new Uri(value.Videos.FirstOrDefault().Url, UriKind.RelativeOrAbsolute);

                    }

                }

        }



        private async void Media_Tapped(object sender, TappedRoutedEventArgs e)

        {

            _tapscount++;

            await Task.Delay(350);

            if (_tapscount == 0) return;

            if (_tapscount == 1)

            {

                if (CarouVideo.Source != null)

                {

                    if (CarouVideo.CurrentState == MediaElementState.Playing)

                        CarouVideo.IsMuted = !CarouVideo.IsMuted;

                    else CarouVideo.Play();

                }
            }
            _tapscount = 0;
        }
    }
}