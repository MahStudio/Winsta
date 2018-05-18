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

namespace WinGoTag.UserControls.DirectMessageUCs
{
    public sealed partial class DirectThreadItemProviderUC : UserControl, INotifyPropertyChanged
    {
        public InstaDirectInboxThread DirectItem
        {
            get
            {
                return (InstaDirectInboxThread)GetValue(DirectItemProperty);
            }
            set
            {
                SetValue(DirectItemProperty, value);
                this.DataContext = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DirectItem"));
            }
        }
        public static readonly DependencyProperty DirectItemProperty = DependencyProperty.Register(
         "DirectItem",
         typeof(InstaDirectInboxThread),
         typeof(DirectThreadItemProviderUC),
         new PropertyMetadata(null)
        );

        public event PropertyChangedEventHandler PropertyChanged;

        public DirectThreadItemProviderUC()
        {
            this.InitializeComponent();
            this.DataContextChanged += DirectMessageUCs_DataContextChanged;
        }

        private void DirectMessageUCs_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            try
            {
                if (args.NewValue.GetType() == typeof(InstaDirectInboxThread))
                {
                    //var value = DataContext as InstaMedia;
                    if(DirectItem.Users.Count > 1)
                    {
                        User.Visibility = Visibility.Collapsed;
                        Group.Visibility = Visibility.Visible;
                    }

                }
            }
            catch { }
        }
    }
}
