using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using WinGoTag.View;
using WinGoTag.View.SignInSignUp;
using System.Diagnostics;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.UI.Xaml;

namespace WinGoTag.ViewModel.SignInSignUp
{
    class LoginViewModel : INotifyPropertyChanged
    {
        #region Properties
        private string _username;
        private string _password;
        private void UpdateProperty(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string UserName
        {
            get { return _username; }
            set { _username = value; UpdateProperty("UserName"); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; UpdateProperty("Password"); }
        }

        Visibility _visibility = Visibility.Collapsed;
        public Visibility LoadingVisibility
        {
            get { return _visibility; }
            set { _visibility = value; UpdateProperty("LoadingVisibility"); }
        }
        bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; UpdateProperty("IsLoading"); }
        }
        #endregion

        #region Commands
        public AppCommand LoginCmd { get; set; }
        public AppCommand RegisterCmd { get; set; }
        #endregion

        CoreDispatcher Dispatcher { get; set; }

        #region WebView Challenge Properties and events
        readonly Uri InstagramUri = new Uri("https://www.instagram.com/");
        bool IsWebBrowserInUse = false;
        private WebView _webView;
        public WebView WebView
        {
            get { return _webView; }
            set { _webView = value; UpdateProperty("WebView"); }
        }
        public void AddWebViewEvents()
        {
            if (WebView == null)
                return;
            try
            {
                WebView.NavigationCompleted += WebViewNavigationCompleted;
                WebView.DOMContentLoaded += WebViewDOMContentLoaded;
                WebView.NewWindowRequested += WebViewNewWindowRequested;
                WebView.NavigationStarting += WebViewNavigationStarting;
            }
            catch { }
        }
        public void DeleteWebViewEvents()
        {
            if (WebView == null)
                return;
            try
            {
                WebView.NavigationCompleted -= WebViewNavigationCompleted;
                WebView.DOMContentLoaded -= WebViewDOMContentLoaded;
                WebView.NewWindowRequested -= WebViewNewWindowRequested;
                WebView.NavigationStarting -= WebViewNavigationStarting;
            }
            catch { }
        }

        private void WebViewNewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            try
            {
                WebView.Navigate(args.Uri);
                args.Handled = true;
            }
            catch { }
        }

        private void WebViewNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            LoadingOn();
        }

        private async void WebViewDOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            try
            {
                LoadingOff();
                if (args.Uri.ToString() == InstagramUri.ToString() && !IsWebBrowserInUse)
                {
                    var cookies = GetBrowserCookie(args.Uri);
                    var sb = new StringBuilder();
                    foreach (var item in cookies)
                    {
                        sb.Append($"{item.Name}={item.Value}; ");
                    }
                    string html = await WebView.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
                    var result = AppCore.InstaApi.SetCookiesAndHtmlForChallenge(html, sb.ToString());
                    WebView.Visibility = Visibility.Collapsed;
                    IsWebBrowserInUse = true;
                    if (result.Succeeded)
                    {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
                        {
                            AppCore.SaveUserInfo(UserName, Password);
                            MainPage.MainFrame.Navigate(typeof(MainView));
                        });
                    }
                    else
                    {
                        await new MessageDialog($"{UserName} couldn't login.\r\nPlease try again.", "Unknown error").ShowAsync();
                    }
                    WebView.Stop();
                }
            }
            catch (Exception) { }
        }

        private async void WebViewNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            try
            {
                await WebView.InvokeScriptAsync("eval", new[]
                 {
                    @"(function()
                    {
                        var hyperlinks = document.getElementsByTagName('a');
                        for(var i = 0; i < hyperlinks.length; i++)
                        {
                            if(hyperlinks[i].getAttribute('target') != null)
                            {
                                hyperlinks[i].setAttribute('target', '_self');
                            }
                        }
                    })()"
                });
            }
            catch { }
        }

        private HttpCookieCollection GetBrowserCookie(Uri targetUri)
        {
            var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            var cookieManager = httpBaseProtocolFilter.CookieManager;
            var cookieCollection = cookieManager.GetCookies(targetUri);
            return cookieCollection;
        }
        #endregion

        public LoginViewModel()
        {
            LoginCmd = AppCommand.GetInstance();
            RegisterCmd = AppCommand.GetInstance();
            LoginCmd.ExecuteFunc = RunLogin;
            RegisterCmd.ExecuteFunc = RunRegister;
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            LoadPage();
        }

        private async void LoadPage()
        {
            await AppCore.RunAsync();
            await Task.Delay(500);
            if (AppCore.InstaApi != null)
            {
                if (AppCore.InstaApi.IsUserAuthenticated)
                {
                    MainPage.MainFrame.Navigate(typeof(MainView));
                    return;
                }
            }
        }

        public async void RunLogin(object obj)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoginAsync);
        }

        private async void LoginAsync()
        {
            LoadingOn();
            UserSessionData User = new UserSessionData()
            {
                UserName = UserName,
                Password = Password
            };
            AppCore.InstaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(User)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .Build();
            var loginres = await AppCore.InstaApi.LoginAsync();
            switch (loginres.Value)
            {
                case InstaLoginResult.ChallengeRequired:
                     await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                     {
                         var challenge = AppCore.InstaApi.GetChallenge();
                         if (WebView != null && challenge != null)
                         {
                             LoadingOn();
                             WebView.Visibility = Visibility.Visible;
                             IsWebBrowserInUse = false;
                             WebView.Navigate(new Uri(challenge.Url));
                         }
                         else
                             LoadingOff();
                     });
                    break;
                case InstaLoginResult.Success:
                    LoadingOff();
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
                    {
                        AppCore.SaveUserInfo(User.UserName, User.Password);
                        MainPage.MainFrame.Navigate(typeof(MainView));
                    });
                    break;
                case InstaLoginResult.BadPassword:
                    LoadingOff();
                    await new MessageDialog(loginres.Info.Message).ShowAsync();
                    break;
                case InstaLoginResult.InvalidUser:
                    LoadingOff();
                    await new MessageDialog(loginres.Info.Message).ShowAsync();
                    break;
                case InstaLoginResult.TwoFactorRequired:
                    LoadingOff();
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
                    {
                        AppCore.SaveUserInfo(User.UserName, User.Password, false);
                        MainPage.MainFrame.Navigate(typeof(TwoStepFactorView));
                    });
                    break;
                case InstaLoginResult.Exception:
                    LoadingOff();
                    await new MessageDialog(loginres.Info.Exception.Message).ShowAsync();
                    break;
                default:
                    break;
            }
        }

        private void RunRegister(object obj)
        {

        }

        private void LoadingOn()
        {
            IsLoading = true;
            LoadingVisibility = Visibility.Visible;
        }

        private void LoadingOff()
        {
            IsLoading = false;
            LoadingVisibility = Visibility.Collapsed;
        }
    }
}
