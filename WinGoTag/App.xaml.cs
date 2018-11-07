using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using WinGoTag.Helpers;

namespace WinGoTag
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            if (!ClassProInfo.SystemVersion.StartsWith("10.0.14393"))
                this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.UnhandledException += App_UnhandledException;
            App.Current.Suspending += Current_Suspending;
            try
            {
                UserAgentHelper.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");
            }
            catch { }
        }

        private async void Current_Suspending(object sender, SuspendingEventArgs e)
        {
            try
            {
                var def = e.SuspendingOperation.GetDeferral();
                if (AppCore.InstaApi != null && AppCore.InstaApi.IsUserAuthenticated)
                {
                    var state = AppCore.InstaApi.GetStateDataAsStream();
                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("UserSession.dat", CreationCollisionOption.ReplaceExisting);
                    using (var stream = AppCore.InstaApi.GetStateDataAsStream())
                    {
                        var mem = new MemoryStream();
                        await state.CopyToAsync(mem);
                        var fst = (await file.OpenAsync(FileAccessMode.ReadWrite));
                        await fst.WriteAsync(mem.GetWindowsRuntimeBuffer());
                    }
                }
                def.Complete();
            }
            catch { }
        }

        private async void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            await new MessageDialog($"{e.Message}{Environment.NewLine}{e.Exception.StackTrace}").ShowAsync();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (ClassProInfo.SystemVersion.StartsWith("10.0.14393"))
            {
                #region Initialize Page
                Application.LoadComponent(this, new Uri("ms-appx:///App.AUSupport.xaml", UriKind.RelativeOrAbsolute), ComponentResourceLocation.Application);
                #endregion
            }
            if (e != null)
            {
                if (e.PreviousExecutionState == ApplicationExecutionState.Running)
                {
                    Window.Current.Activate();
                    return;
                }
            }
            ApplicationData.Current.LocalFolder.Path.ShowInOutput();
            try
            {
                AppCore.UnregisterNotifyTask();

            }
            catch { }
            try
            {
                AppCore.RegisterNotifyTask();
            }
            catch { }
            SplashScreen splashScreen = e.SplashScreen;
            ExtendedSplashScreen eSplash = null;
            eSplash = new ExtendedSplashScreen(splashScreen);
            Window.Current.Content = eSplash;
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
