using InstaSharper.Classes.Models;
using System.Collections.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinGoTag.DataBinding;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.UserViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserFollowersView : Page
    {
        public string UserName
        {
            get => (string)GetValue(UsernameProperty);
            set
            {
                SetValue(UsernameProperty, value);
                DataContext = value;
            }
        }
        public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register(
         "Username",
         typeof(string),
         typeof(UserFollowersView),
         new PropertyMetadata(null)
        );
        public GenerateUserFollowers<InstaUserShort> PageItemssource;
        public UserFollowersView() => InitializeComponent();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(Frame.GoBack);
            if (PageItemssource != null)
                PageItemssource.CollectionChanged -= PageItemssource_CollectionChanged;

            PageItemssource = new GenerateUserFollowers<InstaUserShort>(100000, (count) =>
            {
                // return tres[count];
                return new InstaUserShort();
            }, e.Parameter.ToString());
            PageItemssource.CollectionChanged += PageItemssource_CollectionChanged;
            myList.ItemsSource = PageItemssource;
        }

        void PageItemssource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        void ToBackBT_Click(object sender, RoutedEventArgs e) => Return();

        void User_ItemClick(object sender, ItemClickEventArgs e) => Frame.Navigate(typeof(UserProfileView), e.ClickedItem);

        public void Return()
        {
            Frame.GoBack();
            AppCore.ModerateBack("");
        }
    }
}