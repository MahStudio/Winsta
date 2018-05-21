using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.SearchView
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class TopLiveVideos : Page
    {
        public TopLiveVideos()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //this.DataContext = e.Parameter as InstaTopLive;
            var top = await AppCore.InstaApi.LiveProcessor.GetDiscoverTopLiveAsync();
            
            AdaptiveGridViewControl.ItemsSource = top.Value.Broadcasts;
            //var src = await FFmpegInteropMSS.CreateFromUriAsync(top.Value.Broadcasts.FirstOrDefault().RtmpPlaybackUrl, new FFmpegInteropConfig() {  });
            //var src2 = src.GetMediaStreamSource();
            //Element.SetMediaStreamSource(src2);
            //Element.Play();
        }

        private void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void AdaptiveGridViewControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            var data = ((InstaBroadcast)e.ClickedItem);
            GridViewItem itemAnimation = (GridViewItem)AdaptiveGridViewControl.ContainerFromItem(data);
            
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("Poster", itemAnimation);
            MainPage.MainFrame.Navigate(typeof(StoryViewLive), data);
        }
    }
}
