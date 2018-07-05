using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace WinGoTag.ViewModel.SettingsViewModel
{
    class ChangePasswordVM : INotifyPropertyChanged
    {
        private bool _isbusy;
        private string _oldpass;
        private string _newpass;
        private string _newpass2;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy { get => _isbusy; set { _isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled")); } }
        public string OldPassword
        {
            get => _oldpass;
            set
            {
                _oldpass = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OldPassword"));
            }
        }
        public string NewPassword
        {
            get => _newpass;
            set
            {
                _newpass = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewPassword"));
            }
        }
        public string NewPassword2
        {
            get => _newpass2;
            set
            {
                _newpass2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewPassword2"));
            }
        }

        public bool IsEnabled => !IsBusy;

        public AppCommand ChangePasswordCmd { get; set; }

        CoreDispatcher Dispatcher;
        public ChangePasswordVM()
        {
            IsBusy = false;
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            ChangePasswordCmd = AppCommand.GetInstance();
            ChangePasswordCmd.ExecuteFunc = ChangePass;
        }

        private async void ChangePass(object obj)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, ChangePassFunc);
        }
        
        async void ChangePassFunc()
        {
            IsBusy = true;
            if (NewPassword != NewPassword2)
            {
                await new MessageDialog("New password and repeat of new password must be the same.").ShowAsync();
                IsBusy = false;
                return;
            }
            if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(NewPassword2))
            {
                await new MessageDialog("All fields are required for changing password.").ShowAsync();
                IsBusy = false;
                return;
            }
            var res = await AppCore.InstaApi.ChangePasswordAsync(OldPassword, NewPassword);
            if (res.Value != true)
            {
                await new MessageDialog(res.Info.Message).ShowAsync();
                IsBusy = false;
                return;
            }
            else
            {
                OldPassword = NewPassword = NewPassword2 = "";
                IsBusy = false;
            }
        }

    }
}
