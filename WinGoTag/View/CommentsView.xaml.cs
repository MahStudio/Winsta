using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinGoTag.DataBinding;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CommentsView : Page
    {
        string MediaID = "";
        public GenerateComments<InstaComment> PageItemssource;
        public CommentsView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            AppCore.ModerateBack(Frame.GoBack);
            MediaID = e.Parameter.ToString();
            if (PageItemssource != null)
            {
                PageItemssource.CollectionChanged -= PageItemssource_CollectionChanged;
            }

            PageItemssource = new GenerateComments<InstaComment>(100000, (count) =>
            {
                //return tres[count];
                return new InstaComment();
            }, e.Parameter.ToString());

            PageItemssource.CollectionChanged += PageItemssource_CollectionChanged;
            mylist.ItemsSource = PageItemssource;
        }

        private void PageItemssource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
