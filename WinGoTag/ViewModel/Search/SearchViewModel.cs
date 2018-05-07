using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinGoTag.DataBinding;
using WinGoTag.View.SearchView;

namespace WinGoTag.ViewModel.Search
{
    class SearchViewModel : INotifyPropertyChanged
    {
        private bool _isbusy;

        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsBusy
        {
            get { return _isbusy; }
            set { _isbusy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBusy")); }
        }

        CoreDispatcher Dispatcher { get; set; }
        public SearchViewModel()
        {
            
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            RunLoadPage();
        }

        async void RunLoadPage()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, LoadPage);
        }

        void LoadPage()
        {
           
        }

    }
}
