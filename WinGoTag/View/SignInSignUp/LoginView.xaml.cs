using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using WinGoTag.Helpers;
using WinGoTag.ViewModel.SignInSignUp;


namespace WinGoTag.View.SignInSignUp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginView : Page
    {
        public LoginView()
        {
            this.InitializeComponent();
            Loaded += LoginViewLoaded;
        }
        private void LoginViewLoaded(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                if (DataContext is LoginViewModel context)
                {
                    if (context != null)
                    {
                        context.WebView = WebViewChallenge;
                        context.AddWebViewEvents();

                        context.WebViewFacebook = WebViewFacebook;
                        context.AddWebViewFacebookEvents();
                    }
                }
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            try
            {
                if (DataContext is LoginViewModel context)
                {
                    if (context != null)
                    {
                        context.DeleteWebViewEvents();
                        context.DeleteWebViewFacebookEvents();
                    }
                }
            }
            catch { }
        }

        private void UsernameTextKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (sender != null)
            {
                try
                {
                    if (e.Key == Windows.System.VirtualKey.Enter)
                    {
                        if (string.IsNullOrEmpty(UsernameText.Text))
                            UsernameText.Focus(FocusState.Programmatic);
                        else
                            PasswordText.Focus(FocusState.Programmatic);
                    }
                }
                catch { }
            }
        }

        private void PasswordTextKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (sender != null)
            {
                try
                {
                    if (e.Key == Windows.System.VirtualKey.Enter)
                    {
                        if (string.IsNullOrEmpty(UsernameText.Text))
                            UsernameText.Focus(FocusState.Programmatic);
                        else if (string.IsNullOrEmpty(PasswordText.Password))
                            PasswordText.Focus(FocusState.Programmatic);
                        else
                        {
                            if (DataContext is LoginViewModel context)
                            {
                                if (context != null)
                                {
                                    context.RunLogin(null);
                                }
                            }
                        }
                    }
                }
                catch { }
            }
        }


    }
}
