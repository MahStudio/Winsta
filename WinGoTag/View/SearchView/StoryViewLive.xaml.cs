using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class StoryViewLive : Page
    {
        InstaBroadcast value;
        //private FFmpegInteropMSS FFmpegMSS;
        public StoryViewLive()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPage.Bar.Visibility = Visibility.Collapsed;
            var Data = ((InstaBroadcast)e.Parameter);
            this.DataContext = Data;
            AnimationEnter();
            value = Data;
            //Element.SetMediaStreamSource(FFmpegMSS.GetMediaStreamSource());
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            MainPage.Bar.Visibility = Visibility.Visible;
        }

        public void AnimationEnter()
        {
            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("Poster");
            if (imageAnimation != null)
            { imageAnimation.TryStart(Frame); }
        }

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
