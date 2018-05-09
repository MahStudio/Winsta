using InstaSharper.Classes.Models;
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

namespace WinGoTag.View.UserViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserFollowingsView : Page
    {
        public string UserName
        {
            get
            {
                return (string)GetValue(UsernameProperty);
            }
            set
            {
                SetValue(UsernameProperty, value);
                this.DataContext = value;
            }
        }
        public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register(
         "Username",
         typeof(string),
         typeof(UserFollowingsView),
         new PropertyMetadata(null)
        );
        public GenerateUserFollowings<InstaUserShort> PageItemssource;
        public UserFollowingsView()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (PageItemssource != null)
            {
                PageItemssource.CollectionChanged -= PageItemssource_CollectionChanged;
            }
            PageItemssource = new GenerateUserFollowings<InstaUserShort>(100000, (count) =>
            {
                //return tres[count];
                return new InstaUserShort();
            }, e.Parameter.ToString());
            PageItemssource.CollectionChanged += PageItemssource_CollectionChanged;
            myList.ItemsSource = PageItemssource;
        }

        private void PageItemssource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                var list = PageItemssource[0] as InstaUserShort;
                if ((e.NewItems[0] as InstaUserShort).Pk == list.Pk)
                {
                    PageItemssource.HasMoreItems = false;
                }
            }
            catch { }
        }

        private void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void User_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(UserProfileView), e.ClickedItem);
        }
    }
}
