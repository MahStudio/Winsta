using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace WinGoTag.ViewModel.DirectMessages
{
    class DirectsListViewModel : INotifyPropertyChanged
    {
        private bool _isbusy;
        private InstaDirectInboxContainer _container;
        public event PropertyChangedEventHandler PropertyChanged;
        public InstaDirectInboxContainer InboxContainer { get { return _container; } set { _container = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InboxContainer")); } }
        CoreDispatcher Dispatcher { get; set; }
        public bool IsBusy { get { return _isbusy; } set { _isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); } }
        public DirectsListViewModel()
        {
            IsBusy = true;
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            RunLoadPage();
        }

        async void RunLoadPage()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);
        }

        async void LoadPage()
        {
            var inb = await AppCore.InstaApi.GetDirectInboxAsync();
            InboxContainer = inb.Value;
            IsBusy = false;
        }
    }
}
