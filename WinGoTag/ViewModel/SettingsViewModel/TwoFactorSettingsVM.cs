using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace WinGoTag.ViewModel.SettingsViewModel
{
    class TwoFactorSettingsVM : INotifyPropertyChanged
    {
        private bool _isbusy;
        private bool _requiresecuritycode;
        private List<string> _backupcodes;
        private bool _istwofactorenabled;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy { get => _isbusy; set { _isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled")); } }
        public bool RequireSecurityCode
        {
            get => _requiresecuritycode;
            set
            {
                _requiresecuritycode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RequireSecurityCode"));
            }
        }
        public List<string> BackupCodes
        {
            get => _backupcodes;
            set
            {
                _backupcodes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BackupCodes"));
            }
        }
        public bool IsTwoFactorEnabled
        {
            get => _istwofactorenabled;
            set
            {
                _istwofactorenabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsTwoFactorEnabled"));
            }
        }

        public bool IsEnabled => !IsBusy;

        public AppCommand ChangePasswordCmd { get; set; }

        CoreDispatcher Dispatcher;
        public TwoFactorSettingsVM()
        {
            IsBusy = false;
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            RunLoadPage();
            ChangePasswordCmd = AppCommand.GetInstance();
            ChangePasswordCmd.ExecuteFunc = ChangePass;
        }

        async void RunLoadPage()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);
        }

        async void LoadPage()
        {
            IsBusy = true;
            var res = await AppCore.InstaApi.AccountProcessor.GetSecuritySettingsInfoAsync();
            BackupCodes = res.Value.BackupCodes;
            IsTwoFactorEnabled = res.Value.IsTwoFactorEnabled;
            var st = res.Value.Status;
            IsBusy = false;
        }

        private async void ChangePass(object obj)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, ChangePassFunc);
        }

        async void ChangePassFunc()
        {
            //IsBusy = true;
            //if (NewPassword != NewPassword2)
            //{
            //    await new MessageDialog("New password and repeat of new password must be the same.").ShowAsync();
            //    IsBusy = false;
            //    return;
            //}
            //if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(NewPassword2))
            //{
            //    await new MessageDialog("All fields are required for changing password.").ShowAsync();
            //    IsBusy = false;
            //    return;
            //}
            //var res = await AppCore.InstaApi.ChangePasswordAsync(OldPassword, NewPassword);
            //if (res.Value != true)
            //{
            //    await new MessageDialog(res.Info.Message).ShowAsync();
            //    IsBusy = false;
            //    return;
            //}
            //else
            //{
            //    OldPassword = NewPassword = NewPassword2 = "";
            //    IsBusy = false;
            //}
        }

    }
}
