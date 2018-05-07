using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplashScreen : Page
    {
        public ExtendedSplashScreen(SplashScreen splash, object parameter = null)
        {
            this.InitializeComponent();
            this.Loaded += ExtendedSplashScreen_Loaded;
        }

        private async void ExtendedSplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, LoadPage);
        }

        private async void LoadPage()
        {
            await AppCore.RunAsync();
            RemoveExtendedSplash();
        }
        async void RemoveExtendedSplash()
        {
            try
            {
                Frame frame = new Frame();
                await Task.Delay(1500);
                Window.Current.Content = frame;
                frame.Navigate(typeof(MainPage));
                Window.Current.Activate();
            }
            catch { }
        }
    }
}
