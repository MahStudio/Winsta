using System;
using System.ComponentModel;
using Windows.UI.Core;

namespace WinGoTag.ViewModel.Search
{
    class SearchViewModel : INotifyPropertyChanged
    {
        bool isbusy;

        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsBusy
        {
            get => isbusy;
            set { isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); }
        }

        CoreDispatcher Dispatcher { get; set; }
        public SearchViewModel()
        {
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            RunLoadPage();
        }

        async void RunLoadPage() => await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);

        void LoadPage()
        {
        }
    }
}