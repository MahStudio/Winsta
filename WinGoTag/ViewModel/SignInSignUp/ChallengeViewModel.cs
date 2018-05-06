using InstaAPI.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace WinGoTag.ViewModel.SignInSignUp
{
    class ChallengeViewModel : INotifyPropertyChanged
    {
        private bool? _isemailchoosed;
        private bool? _isphonechoosed;
        private bool _isbusy;
        private Step_Data _choptions;
        public event PropertyChangedEventHandler PropertyChanged;
        public Step_Data ChallengeOptions { get { return _choptions; } set { _choptions = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChallengeOptions")); } }
        public bool IsBusy { get { return _isbusy; } set { _isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); } }
        public bool? IsEmailChoosed { get { return _isemailchoosed; } set { _isemailchoosed = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEmailChoosed")); } }
        public bool? IsPhoneChoosed { get { return _isphonechoosed; } set { _isphonechoosed = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsPhoneChoosed")); } }
        CoreDispatcher Dispatcher;

        public AppCommand SendVerificationCmd { get; set; }
        public ChallengeViewModel()
        {
            IsBusy = true;
            IsEmailChoosed = true;
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            RunLoadPage();
            SendVerificationCmd = AppCommand.GetInstance();
            SendVerificationCmd.ExecuteFunc = SendVerification;
        }

        private async void SendVerification(object obj)
        {
            if (IsEmailChoosed == true)
                await AppCore.InstaApi.SendVerifyForChallenge(1);
            if(IsPhoneChoosed == true)
                await AppCore.InstaApi.SendVerifyForChallenge(2);

        }

        private async void RunLoadPage()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);
        }

        private async void LoadPage()
        {
            var res = await AppCore.InstaApi.GetChallengeChoices();
            if (res.Succeeded)
            {
                ChallengeOptions = res.Value;
            }
            else
            {
                await new MessageDialog(res.Info.Message).ShowAsync();
            }
            IsBusy = false;
        }
    }
}
