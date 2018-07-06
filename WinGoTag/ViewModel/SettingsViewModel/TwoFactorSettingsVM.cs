using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
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

        public AppCommand CopyBackupCodesCmd { get; set; }
        public AppCommand RegenerateBackupCodesCmd { get; set; }

        CoreDispatcher Dispatcher;
        public TwoFactorSettingsVM()
        {
            IsBusy = false;
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            RunLoadPage();
            CopyBackupCodesCmd = AppCommand.GetInstance();
            RegenerateBackupCodesCmd = AppCommand.GetInstance();
            CopyBackupCodesCmd.ExecuteFunc = CopyBackupCodes;
            RegenerateBackupCodesCmd.ExecuteFunc = RegenerateBackupCodes;
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
        
        void CopyBackupCodes(object obj)
        {
            var dp = new DataPackage();
            string BC = "";
            foreach (var item in BackupCodes)
            {
                BC += item + Environment.NewLine; 
            }
            dp.SetText(BC);
            Clipboard.SetContent(dp);
        }

        async void RegenerateBackupCodes(object obj)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, RegenerateBackupCodes);
        }

        async void RegenerateBackupCodes()
        {
            IsBusy = true;
            var b = await AppCore.InstaApi.AccountProcessor.RegenerateTwoFactorBackupCodes();
            if(b.Succeeded && b.Value != null)
            {
                BackupCodes = b.Value.BackupCodes.ToList();
            }
            IsBusy = false;
        }

    }
}
