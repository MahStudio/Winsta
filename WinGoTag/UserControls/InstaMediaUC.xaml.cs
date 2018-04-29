using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinGoTag.UserControls
{
    public sealed partial class InstaMediaUC : UserControl, INotifyPropertyChanged
    {
        public InstaMedia Media
        {
            get
            {
                return (InstaMedia)GetValue(MediaProperty);
            }
            set
            {
                SetValue(MediaProperty, value);
                this.DataContext = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Media"));
            }
        }
        public static readonly DependencyProperty MediaProperty = DependencyProperty.Register(
         "Media",
         typeof(InstaMedia),
         typeof(InstaMediaUC),
         new PropertyMetadata(null)
        );

        public event PropertyChangedEventHandler PropertyChanged;

        public InstaMediaUC()
        {
            this.InitializeComponent();
        }

        private async void LikeBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!Media.HasLiked)
            {
                if ((await AppCore.InstaApi.LikeMediaAsync(Media.InstaIdentifier)).Value)
                    Media.HasLiked = true;
            }
            else
            {
                if ((await AppCore.InstaApi.UnLikeMediaAsync(Media.InstaIdentifier)).Value)
                    Media.HasLiked = false;
            }
        }

        private void CommentBTN_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
