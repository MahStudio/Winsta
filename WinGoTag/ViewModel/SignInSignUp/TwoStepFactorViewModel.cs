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

namespace WinGoTag.ViewModel.SignInSignUp
{
    class TwoStepFactorViewModel : INotifyPropertyChanged
    {
        #region Properties
        private string _verificationcode;
        private bool _isbusy;
        private void UpdateProperty(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string VerificationCode
        {
            get { return _verificationcode; }
            set { _verificationcode = value; UpdateProperty("VerificationCode"); }
        }
        public bool IsBusy
        {
            get
            {
                return _isbusy;
            }
            set { _isbusy = value; UpdateProperty("IsBusy"); }
        }
        #endregion

        #region Commands
        public AppCommand LoginCmd { get; set; }
        #endregion
        CoreDispatcher Dispatcher { get; set; }

        public TwoStepFactorViewModel()
        {
            LoginCmd = AppCommand.GetInstance();
            LoginCmd.ExecuteFunc = RunLogin;
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        }

        private async void RunLogin(object obj)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoginAsync);
        }

        private async void LoginAsync()
        {
            var res = await AppCore.InstaApi.TwoFactorLoginAsync(VerificationCode);
            switch (res.Value)
            {
                case InstaLoginTwoFactorResult.Success:
                    MainPage.MainFrame.Navigate(typeof(MainView));
                    //NavigateToMainView
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
