using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using WinGoTag.DataBinding;

namespace WinGoTag.ViewModel.DirectMessages
{
    class DirectsListViewModel : INotifyPropertyChanged
    {
        private bool _isbusy;
        private GenerateDirectsList<InstaDirectInboxThread> _threads;
        public event PropertyChangedEventHandler PropertyChanged;
        public GenerateDirectsList<InstaDirectInboxThread> InboxThreads { get => _threads; set { _threads = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InboxThreads")); } }
        CoreDispatcher Dispatcher { get; set; }
        public bool IsBusy { get => _isbusy; set { _isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); } }
        public DirectsListViewModel()
        {
            IsBusy = true;
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            RunLoadPage();
        }

        async void RunLoadPage() => await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);

        async void LoadPage()
        {
            InboxThreads = new GenerateDirectsList<InstaDirectInboxThread>(100000, (count) => new InstaDirectInboxThread());

            if (ClassInfo.DeviceType() == ClassInfo.DeviceTypeEnum.Phone)
                await InboxThreads.LoadMoreItemsAsync(1);

            IsBusy = false;
        }
    }
}
