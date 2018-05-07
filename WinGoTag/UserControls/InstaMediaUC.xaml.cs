using InstaSharper.API.Processors;
using InstaSharper.Classes.Models;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
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
                    //var value = DataContext as InstaMedia;
                    switch (Media.MediaType)
                    {
                        case InstaMediaType.Image:
                            break;

                        case InstaMediaType.Carousel:
                            //ThisFlipView.Height = Media.Carousel[0].Images[0].Height;
                            break;

                        case InstaMediaType.Video:
                            break;
                    }

                     
                   

                    if (Media.LikesCount == 0)
                    { LikeCount.Visibility = Visibility.Collapsed; }

                    if (Media.CommentsCount == "0")
                    { HyperComment.Visibility = Visibility.Collapsed; }

                    LikeCount.Text = $"{Media.LikesCount} people like it";
                    CommentCount.Text = "See all " + Media.CommentsCount + " comments";
               

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

        private void Cover_ImageOpened(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fade = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3),
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(fade, (Image)sender);
            Storyboard.SetTargetProperty(fade, "Opacity");
            Storyboard openpane = new Storyboard();
            openpane.Children.Add(fade);
            openpane.Begin();
        }


        private void AddtoCollection_Click(object sender, RoutedEventArgs e)
        {
            // get all collections of current user
            //var collections = await AppCore.InstaApi.GetCollectionsAsync();
            // get specific collection by id
            //var collectionId = 1234567890;
            //var collection = await AppCore.InstaApi.GetCollectionAsync(collectionId);
            // create collection
            //var createResult = await AppCore.InstaApi.CreateCollectionAsync("My collection");
            // add items to collection
            //var mediaItems = new[] { "2658893121999767931" };
            //var addResult = await AppCore.InstaApi.AddItemsToCollectionAsync(collectionId, mediaItems);
            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, AddToCollectionRunner);
        }

        private async void AddToCollectionRunner()
        {
            var AddToCollection = await AppCore.InstaApi.AddItemsToCollectionAsync(0, Media.Caption.MediaId);
        }

        
    }
}
