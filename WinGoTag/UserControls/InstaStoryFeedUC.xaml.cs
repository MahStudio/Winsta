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
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinGoTag.UserControls
{
    public sealed partial class InstaStoryFeedUC : UserControl, INotifyPropertyChanged
    {
        public InstaReelFeed Story
        {
            get
            {
                return (InstaReelFeed)GetValue(StoryProperty);
            }
            set
            {
                SetValue(StoryProperty, value);
                this.DataContext = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Media"));
            }
        }
        public static readonly DependencyProperty StoryProperty = DependencyProperty.Register(
         "Story",
         typeof(InstaReelFeed),
         typeof(InstaStoryFeedUC),
         new PropertyMetadata(null)
        );

        public event PropertyChangedEventHandler PropertyChanged;

        public InstaStoryFeedUC()
        {
            this.InitializeComponent();
            //this.DataContextChanged += InstaStoryFeedUC_DataContextChanged;
        }

        //private void InstaStoryFeedUC_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        //{
        //    if (args.NewValue != null)
        //        if(args.NewValue.GetType() == typeof(InstaReelFeed))
        //        {
        //            var value = DataContext as InstaReelFeed;
        //            
        //        }
        //}
    }
}
