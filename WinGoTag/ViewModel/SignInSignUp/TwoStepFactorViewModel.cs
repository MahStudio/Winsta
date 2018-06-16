using InstaSharper.Classes;
using System;
using System.ComponentModel;
using Windows.UI.Core;
using Windows.UI.Popups;
using WinGoTag.View;

namespace WinGoTag.ViewModel.SignInSignUp
{
    class TwoStepFactorViewModel : INotifyPropertyChanged
    {
        #region Properties
        string verificationcode;
        bool isbusy;

        public TwoStepFactorViewModel()
        {
            LoginCmd = AppCommand.GetInstance();
            LoginCmd.ExecuteFunc = RunLogin;
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        }
        void UpdateProperty(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;
        public string VerificationCode
        {
            get => verificationcode;
            set { verificationcode = value; UpdateProperty("VerificationCode"); }
        }
        public bool IsBusy
        {
            get => isbusy;
            set { isbusy = value; UpdateProperty("IsBusy"); }
        }
        #endregion

        #region Commands
        public AppCommand LoginCmd { get; set; }
        #endregion
        CoreDispatcher Dispatcher { get; set; }

        async void RunLogin(object obj) =>
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoginAsync);

        async void LoginAsync()
        {
            var res = await AppCore.InstaApi.TwoFactorLoginAsync(VerificationCode);
            switch (res.Value)
            {
                case InstaLoginTwoFactorResult.Success:
                    AppCore.SaveUserInfo(null, null, true);
                    MainPage.MainFrame.Navigate(typeof(MainView));
                    // NavigateToMainView
                    break;
                case InstaLoginTwoFactorResult.InvalidCode:
                    await new MessageDialog(res.Info.Message).ShowAsync();
                    break;
                case InstaLoginTwoFactorResult.CodeExpired:
                    await new MessageDialog(res.Info.Message).ShowAsync();
                    break;
                case InstaLoginTwoFactorResult.Exception:
                    await new MessageDialog(res.Info.Exception.Message).ShowAsync();
                    break;
                default:
                    break;
            }
        }
    }
}