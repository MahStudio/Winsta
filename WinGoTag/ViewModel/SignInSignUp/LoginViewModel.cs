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
using WinGoTag.View;
using WinGoTag.View.SignInSignUp;

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
        #endregion

        #region Commands
        public AppCommand LoginCmd { get; set; }
        public AppCommand RegisterCmd { get; set; }
        #endregion

        CoreDispatcher Dispatcher { get; set; }
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

        private async void RunLogin(object obj)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoginAsync);
        }

        private async void LoginAsync()
        {
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
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
                     {
                         AppCore.SaveUserInfo(User.UserName, User.Password, false);
                         MainPage.MainFrame.Navigate(typeof(ChallengeView));
                     });
                    break;
                case InstaLoginResult.Success:
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
                    {
                        AppCore.SaveUserInfo(User.UserName, User.Password);
                        MainPage.MainFrame.Navigate(typeof(MainView));
                    });
                    break;
                case InstaLoginResult.BadPassword:
                    await new MessageDialog(loginres.Info.Message).ShowAsync();
                    break;
                case InstaLoginResult.InvalidUser:
                    await new MessageDialog(loginres.Info.Message).ShowAsync();
                    break;
                case InstaLoginResult.TwoFactorRequired:
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
                    {
                        AppCore.SaveUserInfo(User.UserName, User.Password, false);
                        MainPage.MainFrame.Navigate(typeof(TwoStepFactorView));
                    });
                    break;
                case InstaLoginResult.Exception:
                    await new MessageDialog(loginres.Info.Exception.Message).ShowAsync();
                    break;
                default:
                    break;
            }
        }

        private void RunRegister(object obj)
        {

        }

    }
}
