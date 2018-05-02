using InstaSharper.Classes.Models;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using WinGoTag.ContentDialogs;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinGoTag.UserControls
{
    public sealed partial class InstaMediaUC : UserControl, INotifyPropertyChanged
    {
        public InstaMedia Media
        {
            get
            {
                return (InstaMedia)GetValue(MediaProperty);
            }
            set
            {
                SetValue(MediaProperty, value);
                this.DataContext = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Media"));
            }
        }
        public static readonly DependencyProperty MediaProperty = DependencyProperty.Register(
         "Media",
         typeof(InstaMedia),
         typeof(InstaMediaUC),
         new PropertyMetadata(null)
        );

        public event PropertyChangedEventHandler PropertyChanged;

        int _tapscount = 0;
        public InstaMediaUC()
        {
            this.InitializeComponent();
            this.DataContextChanged += InstaMediaUC_DataContextChanged;
        }

        private async void InstaMediaUC_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            try
            {
                if (args.NewValue.GetType() == typeof(InstaMedia))
                {
                    await Task.Delay(100);
                    Media.PropertyChanged += Media_PropertyChanged;
                    if (!txtCaption.IsTextTrimmed)
                        MoreBTN.Visibility = Visibility.Collapsed;
                    else MoreBTN.Visibility = Visibility.Visible;
                }
            }
            catch { }
        }

        private void Media_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Play")
            {
                if (MedEl.Source != null)
                    if (Media.Play) MedEl.Play();
                    else MedEl.Pause();
            }
        }

        #region Buttons Events
        private async void LikeBTN_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, LikeDislikeRunner);
        }

        private void CommentBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MoreBTN_Click(object sender, RoutedEventArgs e)
        {
            txtCaption.MaxLines = 0;
            (sender as HyperlinkButton).Visibility = Visibility.Collapsed;
        }

        private async void OptionsBTN_Click(object sender, RoutedEventArgs e)
        {
            await new InstaMediaContentDialog(Media).ShowAsync();
        }
        #endregion

        private async void Media_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _tapscount++;
            await Task.Delay(350);
            if (_tapscount == 0) return;
            if (_tapscount == 1)
            {
                if (MedEl.Source != null)
                {
                    if (MedEl.CurrentState == MediaElementState.Playing)
                        MedEl.IsMuted = !MedEl.IsMuted;
                    else MedEl.Play();
                }
            }
            if (_tapscount == 2)
            {
                await Task.Delay(10);
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, LikeDislikeRunner);
            }
            _tapscount = 0;
        }

        private async void LikeDislikeRunner()
        {
            if (!Media.HasLiked)
            {
                if ((await AppCore.InstaApi.LikeMediaAsync(Media.InstaIdentifier)).Value)
                    Media.HasLiked = true;
            }
            else
            {
                if ((await AppCore.InstaApi.UnLikeMediaAsync(Media.InstaIdentifier)).Value)
                    Media.HasLiked = false;
            }
        }
    }
}
