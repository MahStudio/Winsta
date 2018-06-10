using InstaAPI.Classes.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinGoTag.DataBinding;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.StoryView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StoryViewersView : Page
    {
        public GenerateStoryMediaViewers<User1> StoryViewersItemssource;

        public StoryViewersView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AppCore.ModerateBack(Frame.GoBack);
            if (StoryViewersItemssource != null)
            {
                StoryViewersItemssource.CollectionChanged -= StoryViewersItemssource_CollectionChanged;
            }

            StoryViewersItemssource = new GenerateStoryMediaViewers<User1>(100000, (count) =>
            {
                //return tres[count];
                return new User1();
            }, e.Parameter.ToString());

            StoryViewersItemssource.CollectionChanged += StoryViewersItemssource_CollectionChanged;
            //MediasCVS.Source = HomePageItemssource;
            myList.ItemsSource = StoryViewersItemssource;

        }

        private void StoryViewersItemssource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            AppCore.ModerateBack("");
        }
    }
}
