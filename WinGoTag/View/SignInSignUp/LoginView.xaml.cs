using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
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
            InitializeComponent();
            Loaded += LoginViewLoaded;
        }
        void LoginViewLoaded(object sender, RoutedEventArgs e)
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

        void UsernameTextKeyDown(object sender, KeyRoutedEventArgs e)
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

        void PasswordTextKeyDown(object sender, KeyRoutedEventArgs e)
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