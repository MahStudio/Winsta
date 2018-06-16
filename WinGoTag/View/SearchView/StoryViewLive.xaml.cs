using InstaSharper.Classes.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.SearchView
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class StoryViewLive : Page
    {
        InstaBroadcast value;
        // private FFmpegInteropMSS FFmpegMSS;
        public StoryViewLive() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPage.Bar.Visibility = Visibility.Collapsed;
            var data = ((InstaBroadcast)e.Parameter);
            DataContext = data;
            AnimationEnter();
            value = data;
            // Element.SetMediaStreamSource(FFmpegMSS.GetMediaStreamSource());
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) => MainPage.Bar.Visibility = Visibility.Visible;

        public void AnimationEnter()
        {
            var imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("Poster");
            if (imageAnimation != null)
            { imageAnimation.TryStart(Frame); }
        }

        void BackBT_Click(object sender, RoutedEventArgs e) => Frame.GoBack();
    }
}