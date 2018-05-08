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
    public sealed partial class UserFollowersView : Page
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
         typeof(UserFollowersView),
         new PropertyMetadata(null)
        );
        public GenerateUserFollowers<InstaUserShort> PageItemssource;
        public UserFollowersView()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(PageItemssource != null)
            {
                PageItemssource.CollectionChanged -= PageItemssource_CollectionChanged;
            }
            PageItemssource = new GenerateUserFollowers<InstaUserShort>(100000, (count) =>
            {
                //return tres[count];
                return new InstaUserShort();
            }, e.Parameter.ToString());
            PageItemssource.CollectionChanged += PageItemssource_CollectionChanged;
            myList.ItemsSource = PageItemssource;
        }

        private void PageItemssource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }
        
    }
}
