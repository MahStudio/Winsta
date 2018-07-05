using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace WinGoTag.ViewModel.SettingsViewModel
{
    class StorySettingsViewModel : INotifyPropertyChanged
    {
        private bool _isbusy;
        private bool _allowstoryreshare;
        private bool _savetocamerarool;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy { get => _isbusy; set { _isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled")); } }
        public bool AllowStoryReshare
        {
            get => _allowstoryreshare;
            set
            {
                _allowstoryreshare = value;
                if (!IsBusy)
                    AllowStoryReshareFunc(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllowStoryReshare"));
            }
        }
        public bool SaveToCameraRoll
        {
            get => _savetocamerarool;
            set
            {
                _savetocamerarool = value;
                if (!IsBusy)
                    SaveToCameraRollFunc(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SaveToCameraRoll"));
            }
        }

        public bool IsEnabled => !IsBusy;

        CoreDispatcher Dispatcher;
        public StorySettingsViewModel()
        {
            IsBusy = true;
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            RunLoadPage();
        }

        private async void RunLoadPage() => await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);

        private async void LoadPage()
        {
            IsBusy = true;
            var StorySettings = await AppCore.InstaApi.AccountProcessor.GetStorySettingsAsync();
            if (StorySettings.Value != null)
            {
                AllowStoryReshare = StorySettings.Value.AllowStoryReshare;
                SaveToCameraRoll = StorySettings.Value.SaveToCameraRoll;
            }
            IsBusy = false;
        }

        public async void AllowStoryReshareFunc(bool value)
        {
            IsBusy = true;
            await AppCore.InstaApi.AccountProcessor.AllowStorySharingAsync(value);
            LoadPage();
        }
        private async void SaveToCameraRollFunc(bool value)
        {
            IsBusy = true;
            if (value)
                await AppCore.InstaApi.AccountProcessor.EnableSaveStoryToGalleryAsync();
            else await AppCore.InstaApi.AccountProcessor.DisableSaveStoryToGalleryAsync();
            LoadPage();
        }
    }
}
