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
using WinGoTag.Helpers;
using Newtonsoft.Json;
using InstaAPI.Classes;

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
        public AppCommand FacebookLoginCmd { get; set; }

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

        private void DeleteInstagramCookies()
        {
            try
            {

                HttpBaseProtocolFilter myFilter = new HttpBaseProtocolFilter();
                var cookieManager = myFilter.CookieManager;

                HttpCookieCollection myCookieJar = cookieManager.GetCookies(InstagramUri);
                foreach (HttpCookie cookie in myCookieJar)
                {
                    cookieManager.DeleteCookie(cookie);
                }
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
        private void WebViewNewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            try
            {
                WebView.Navigate(args.Uri);
                args.Handled = true;
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

        #region Facebook Login
        DispatcherTimer FacebookTimer = new DispatcherTimer();
        public WebView WebViewFacebook { get; set; }
        bool FacebookFirstTime = true;
        bool FacebookPassed = false;
        const string FacebookBlockedMsg = "Facebook website is blocked(filtered) in your area.\r\nPlease connect to vpn and try again.";
        public async void RunFacebookLogin(object obj)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, NavigateToInstagramForFacebookLogin);
        }
        public void AddWebViewFacebookEvents()
        {
            if (WebViewFacebook == null)
                return;
            try
            {
                FacebookTimer.Interval = TimeSpan.FromSeconds(15);
                FacebookTimer.Tick += FacebookTimerTick;
                WebViewFacebook.NavigationCompleted += WebViewFacebookNavigationCompleted;
                WebViewFacebook.DOMContentLoaded += WebViewFacebookDOMContentLoaded;
                WebViewFacebook.NewWindowRequested += WebViewFacebookNewWindowRequested;
                WebViewFacebook.NavigationStarting += WebViewFacebookNavigationStarting;
            }
            catch { }
        }

        public void DeleteWebViewFacebookEvents()
        {
            if (WebViewFacebook == null)
                return;
            try
            {
                FacebookTimer.Tick -= FacebookTimerTick;
                WebViewFacebook.NavigationCompleted -= WebViewFacebookNavigationCompleted;
                WebViewFacebook.DOMContentLoaded -= WebViewFacebookDOMContentLoaded;
                WebViewFacebook.NewWindowRequested -= WebViewFacebookNewWindowRequested;
                WebViewFacebook.NavigationStarting -= WebViewFacebookNavigationStarting;
            }
            catch { }
        }

        public async void NavigateToInstagramForFacebookLogin()
        {
            if (WebViewFacebook == null)
                return;
            try
            {
                LoadingOn();
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(new Uri("https://m.facebook.com/"));
                    if (!response.IsSuccessStatusCode)
                    {
                        LoadingOff();
                        await new MessageDialog(FacebookBlockedMsg).ShowAsync();
                        return;
                    }
                }
            }
            catch
            {
                LoadingOff();
                await new MessageDialog(FacebookBlockedMsg).ShowAsync();
                return;
            }
            try
            {
                UserAgentHelper.SetUserAgent("Mozilla/5.0 (Linux; Android 4.4; Nexus 5 Build/_BuildID_) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/30.0.0.0 Mobile Safari/537.36");
            }
            catch { }
            DeleteInstagramCookies();
            DeleteFacebookCookies();
            FacebookFirstTime = true;
            FacebookPassed = false;
            LoadingOn();
            WebViewFacebook.Navigate(InstagramUri);
        }
        private void DeleteFacebookCookies()
        {
            try
            {

                HttpBaseProtocolFilter myFilter = new HttpBaseProtocolFilter();
                var cookieManager = myFilter.CookieManager;

                HttpCookieCollection myCookieJar = cookieManager.GetCookies(new Uri("https://facebook.com/"));
                foreach (HttpCookie cookie in myCookieJar)
                {
                    cookieManager.DeleteCookie(cookie);
                }
            }
            catch { }
            try
            {

                HttpBaseProtocolFilter myFilter = new HttpBaseProtocolFilter();
                var cookieManager = myFilter.CookieManager;

                HttpCookieCollection myCookieJar = cookieManager.GetCookies(new Uri("https://www.facebook.com/"));
                foreach (HttpCookie cookie in myCookieJar)
                {
                    cookieManager.DeleteCookie(cookie);
                }
            }
            catch { }
            try
            {

                HttpBaseProtocolFilter myFilter = new HttpBaseProtocolFilter();
                var cookieManager = myFilter.CookieManager;

                HttpCookieCollection myCookieJar = cookieManager.GetCookies(new Uri("https://m.facebook.com/"));
                foreach (HttpCookie cookie in myCookieJar)
                {
                    cookieManager.DeleteCookie(cookie);
                }
            }
            catch { }
        }
        private void WebViewFacebookNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            LoadingOn();
        }

        private async void WebViewFacebookDOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            try
            {
                if (args.Uri.ToString().Contains("facebook.com/") && !FacebookPassed)
                {
                    LoadingOff();
                    WebViewFacebook.Visibility = Visibility.Visible;
                }
                else
                {
                    if (args.Uri.ToString() == InstagramUri.ToString() ||
                        args.Uri.ToString() == "https://www.instagram.com/#reactivated" ||
                        args.Uri.ToString().StartsWith("https://www.instagram.com/accounts/onetap/"))
                    {
                        LoadingOn();
                        WebViewFacebook.Visibility = Visibility.Collapsed;
                        if (FacebookFirstTime)
                        {
                            FacebookFirstTime = false;
                            FacebookTimer.Start();
                        }
                        else
                        {
                            string html = await WebViewFacebook.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });

                            var cookies = GetBrowserCookie(args.Uri);
                            var sb = new StringBuilder();
                            foreach (var item in cookies)
                            {
                                sb.Append($"{item.Name}={item.Value}; ");
                            }
                            var start = "<script type=\"text/javascript\">window._sharedData";
                            var end = ";</script>";

                            if (html.Contains(start))
                            {
                                var str = html.Substring(html.IndexOf(start) + start.Length);
                                str = str.Substring(0, str.IndexOf(end));
                                str = str.Substring(str.IndexOf("=") + 2);
                                var o = JsonConvert.DeserializeObject<WebBrowserResponse>(str);
                                if (o.Config.Viewer != null)
                                {
                                    UserName = o.Config.Viewer.Username;
                                    Password = "AlakiMasalanYePassworde";
                                    var userSession = new UserSessionData
                                    {
                                        UserName = UserName,
                                        Password = Password
                                    };
                                 
                                    AppCore.InstaApi = InstaApiBuilder.CreateBuilder()
                                        .SetUser(userSession)
                                        .UseLogger(new DebugLogger(LogLevel.Exceptions))
                                        .Build();

                                    var result = AppCore.InstaApi.SetCookiesAndHtmlForChallenge(html, sb.ToString(), true);

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
                                    WebViewFacebook.Stop();
                                }
                            }
                        }
                    }
                    else
                    {
                        //LoadingOff();
                        //WebViewFacebook.Visibility = Visibility.Visible;
                    }
                }
            }
            catch { }
        }


        private void WebViewFacebookNewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            try
            {
                WebViewFacebook.Navigate(args.Uri);
                args.Handled = true;
            }
            catch { }
        }
        private async void WebViewFacebookNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            try
            {
                await WebViewFacebook.InvokeScriptAsync("eval", new[]
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


        private async void FacebookTimerTick(object sender, object e)
        {
            FacebookTimer.Stop();
            try
            {
                var functionString = string.Format(@"document.getElementsByClassName('_5f5mN       jIbKX KUBKM      yZn4P   ')[0].click();");
                await WebViewFacebook.InvokeScriptAsync("eval", new string[] { functionString });
            }
            catch { }
            try
            {
                var functionString = string.Format(@"document.getElementsByClassName('_5f5mN       jIbKX KUBKM      yZn4P   ')[0].click();");
                await WebViewFacebook.InvokeScriptAsync("eval", new string[] { functionString });
            }
            catch { }
        }
        #endregion

        public LoginViewModel()
        {
            LoginCmd = AppCommand.GetInstance();
            FacebookLoginCmd = AppCommand.GetInstance();
            RegisterCmd = AppCommand.GetInstance();
            LoginCmd.ExecuteFunc = RunLogin;
            FacebookLoginCmd.ExecuteFunc = RunFacebookLogin;
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
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoginAskSessionAsync);
        }
        private async void LoginAskSessionAsync()
        {
            try
            {
                var exists = await SessionHelper.IsBackupSessionAvailable(UserName);
                if (exists)
                {
                    var content = $"It seems you stored session for '{UserName}' account.\r\n" +
                        $"Do you want to use this session?\r\n" +
                        $"Press 'No' if you want to create new session.";
                    var md = new MessageDialog(content);
                    md.Commands.Add(new UICommand("Yes"));
                    md.Commands.Add(new UICommand("No"));
                    md.CancelCommandIndex = 1;
                    md.DefaultCommandIndex = 1;

                    var label = await md.ShowAsync();
                    if (label.Label == "Yes")
                    {
                        var json = await SessionHelper.GetBackupSession(UserName);
                        string User = "username"; string Pass = "password";
                        AppCore.InstaApi = InstaApiBuilder.CreateBuilder()
                            .SetUser(new UserSessionData { UserName = User, Password = Pass })
                            .UseLogger(new DebugLogger(LogLevel.Exceptions))
                            .Build();
                        AppCore.InstaApi.LoadStateDataFromStream(json);
                        if (!AppCore.InstaApi.IsUserAuthenticated)
                        {
                            await AppCore.InstaApi.LoginAsync();
                        }
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
                        {
                            AppCore.SaveUserInfo(UserName, Password);
                            MainPage.MainFrame.Navigate(typeof(MainView));
                        });
                    }
                    else
                    {
                        await SessionHelper.DeleteBackupSession(UserName);
                        LoginAsync();
                    }
                }
                else
                    LoginAsync();
            }
            catch
            {
                LoginAsync();
            }
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
                             try
                             {
                                 UserAgentHelper.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");
                             }
                             catch { }
                             DeleteInstagramCookies();
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
