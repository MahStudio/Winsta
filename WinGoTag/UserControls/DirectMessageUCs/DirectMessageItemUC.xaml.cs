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
using WinGoTag.View.DirectMessages;

// Il modello di elemento Controllo utente è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinGoTag.UserControls.DirectMessageUCs
{
    public sealed partial class DirectMessageItemUC : UserControl, INotifyPropertyChanged
    {
        public InstaDirectInboxItem InboxItem
        {
            get
            {
                return (InstaDirectInboxItem)GetValue(MediaProperty);
            }
            set
            {
                SetValue(MediaProperty, value);
                this.DataContext = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InboxItem"));
            }
        }
        public static readonly DependencyProperty MediaProperty = DependencyProperty.Register(
         "InboxItem",
         typeof(InstaDirectInboxItem),
         typeof(DirectMessageItemUC),
         new PropertyMetadata(null)
        );

        public event PropertyChangedEventHandler PropertyChanged;

        public DirectMessageItemUC()
        {
            this.InitializeComponent();
            this.DataContextChanged += DirectMessageItemUC_DataContextChanged;
        }

        private void DirectMessageItemUC_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            try
            {
                if (args.NewValue.GetType() == typeof(InstaDirectInboxItem))
                {

                    if (InboxItem.UserId == DirectChatView.UserId)
                    {
                        UserAvatar.DataContext = DirectChatView.DataUser;
                        SampleText.Background = null;
                        Post.Background = null;
                        SampleText.HorizontalAlignment = HorizontalAlignment.Left;
                        Post.HorizontalAlignment = HorizontalAlignment.Left;
                    }

                    switch (InboxItem.ItemType)
                    {
                        case InstaDirectThreadItemType.Like:

                            break;

                        case InstaDirectThreadItemType.Link:
                            break;

                        case InstaDirectThreadItemType.Media:
                            
                            Post.Visibility = Visibility.Visible;
                            break;

                        case InstaDirectThreadItemType.MediaShare:
                            //InboxItem.MediaShare.Caption.Text
                            Post.Visibility = Visibility.Visible;
                            break;

                        case InstaDirectThreadItemType.Text:
                            SampleText.Visibility = Visibility.Visible;
                            break;
                    }

                    



                }
            }
            catch { }
        }
    }
}
