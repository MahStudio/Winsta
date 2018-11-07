using InstagramApiSharp.Classes.Models;
using System.Collections.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

        public StoryViewersView() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(Frame.GoBack);
            if (StoryViewersItemssource != null)
                StoryViewersItemssource.CollectionChanged -= StoryViewersItemssource_CollectionChanged;

            StoryViewersItemssource = new GenerateStoryMediaViewers<User1>(100000, (count) => new User1(), e.Parameter.ToString());

            StoryViewersItemssource.CollectionChanged += StoryViewersItemssource_CollectionChanged;
            // MediasCVS.Source = HomePageItemssource;
            myList.ItemsSource = StoryViewersItemssource;
        }

        void StoryViewersItemssource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
            AppCore.ModerateBack("");
        }
    }
}