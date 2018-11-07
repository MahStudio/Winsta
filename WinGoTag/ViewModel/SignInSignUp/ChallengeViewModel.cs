using InstagramApiSharp.Classes;
using System;
using System.ComponentModel;
using Windows.UI.Core;

namespace WinGoTag.ViewModel.SignInSignUp
{
    class ChallengeViewModel : INotifyPropertyChanged
    {
        bool? isemailchoosed;
        bool? isphonechoosed;
        bool isbusy;
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsBusy { get => isbusy; set { isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); } }
        public bool? IsEmailChoosed { get => isemailchoosed; set { isemailchoosed = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEmailChoosed")); } }
        public bool? IsPhoneChoosed { get => isphonechoosed; set { isphonechoosed = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsPhoneChoosed")); } }
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

        void SendVerification(object obj)
        {
            // if (IsEmailChoosed == true)
            //    await AppCore.InstaApi.SendVerifyForChallenge(1);
            // if(IsPhoneChoosed == true)
            //    await AppCore.InstaApi.SendVerifyForChallenge(2);
        }

        async void RunLoadPage() => await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);

        void LoadPage()
        {
            // var res = await AppCore.InstaApi.GetChallengeChoices();
            // if (res.Succeeded)
            // {
            //    ChallengeOptions = res.Value;
            // }
            // else
            // {
            //    await new MessageDialog(res.Info.Message).ShowAsync();
            // }
            IsBusy = false;
        }
    }
}