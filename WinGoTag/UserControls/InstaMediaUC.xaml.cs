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
using WinGoTag.View;
using WinGoTag.View.UserViews;

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
                            CountIndicator.Text = Media.Carousel.Count.ToString();
                            break;

                        case InstaMediaType.Video:
                            break;
                    }

                    if (Media.Location != null)
                    { _LocationT.Visibility = Visibility.Visible; }

                    if (Media.Caption == null)
                    { txtCaption.Visibility = Visibility.Collapsed; }

                    if (Media.LikesCount == 0)
                    { LikeCount.Visibility = Visibility.Collapsed; }

                    if (Media.CommentsCount == "0")
                    { HyperComment.Visibility = Visibility.Collapsed; }

                    //LikeCount.Text = $"{Media.LikesCount} people like it";
                    //CommentCount.Text = "See all " + Media.CommentsCount + " comments";
               

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
        private void Username_Click(object sender, RoutedEventArgs e)
        {
            MainPage.MainFrame.Navigate(typeof(UserProfileView), Media.User);
        }
        private async void LikeBTN_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, LikeDislikeRunner);
        }

        private void CommentBTN_Click(object sender, RoutedEventArgs e)
        {
            MainPage.MainFrame.Navigate(typeof(CommentsView), Media.InstaIdentifier);
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
                LikeAnimation();
            }
            _tapscount = 0;
        }

        private async void LikeDislikeRunner()
        {
            if (!Media.HasLiked)
            {
                if ((await AppCore.InstaApi.LikeMediaAsync(Media.InstaIdentifier)).Value)
                {
                    Media.HasLiked = true;
                    Media.LikesCount += 1;
                }
            }
            else
            {
                if ((await AppCore.InstaApi.UnLikeMediaAsync(Media.InstaIdentifier)).Value)
                {
                    Media.HasLiked = false;
                    Media.LikesCount -= 1;
                }
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

        private void LikeAnimation()
        {
            LikeAnimations.Visibility = Visibility.Visible;
            DoubleAnimation fade = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.6),
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(fade, LikeAnimations);
            Storyboard.SetTargetProperty(fade, "Opacity");
            Storyboard openpane = new Storyboard();
            openpane.Children.Add(fade);
            openpane.Begin();

            Task.Delay(5000);

            DoubleAnimation fadeC = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.6),
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(fadeC, LikeAnimations);
            Storyboard.SetTargetProperty(fadeC, "Opacity");
            Storyboard openpaneC = new Storyboard();
            openpaneC.Children.Add(fadeC);
            openpaneC.Begin();
            //LikeAnimations.Visibility = Visibility.Visible;
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

        private void ThisFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = ThisFlipView.SelectedIndex;
            i++;
            _SelectCount.Text = i.ToString();
            var t = (sender as FlipView).SelectedItem;
            FlipViewItem item2 = (FlipViewItem)ThisFlipView.ContainerFromItem(t);
            if (item2 == null) { return; }
            var h = (CarouselItemUC)item2.ContentTemplateRoot;
            if(h.ActualHeight is 0) { return; }
            ThisFlipView.Height = h.ActualHeight;
        }

        private void ThisFlipView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //var t = (sender as FlipView).SelectedItem;
            //FlipViewItem item2 = (FlipViewItem)ThisFlipView.ContainerFromItem(t);
            //if (item2 == null) { return; }
            //var h = (CarouselItemUC)item2.ContentTemplateRoot;
            //if (h.ActualHeight is 0) { return; }
            //ThisFlipView.Height = h.ActualHeight;
        }
        
    }
}
