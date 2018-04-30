using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinGoTag.UserControls
{
    public sealed partial class CarouselItemUC : UserControl, INotifyPropertyChanged
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
         typeof(CarouselItemUC),
         new PropertyMetadata(null)
        );

        public event PropertyChangedEventHandler PropertyChanged;
        int _tapscount = 0;
        public CarouselItemUC()
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
                    if (value.Videos.Count > 0)
                    {
                        CarouImage.Visibility = Visibility.Collapsed;
                    }
                    else if (value.Images.Count > 0)
                    {
                        CarouVideo.Visibility = Visibility.Collapsed;
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
