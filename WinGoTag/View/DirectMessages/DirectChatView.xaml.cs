using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
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

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.DirectMessages
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class DirectChatView : Page
    {
        public static long UserId;
        public static InstaUserShort DataUser;
        public string ThreadIds;
        public string VieweIds;
        GenerateDirectThreadList<InstaDirectInboxItem> ItemsList;

        List<InstaDirectInboxItem> listTest = new List<InstaDirectInboxItem>();
        public DirectChatView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(BackFunc);
            this.DataContext = ((InstaDirectInboxThread)e.Parameter);
            var source = ((InstaDirectInboxThread)e.Parameter);
            ItemsList = new GenerateDirectThreadList<InstaDirectInboxItem>(100000, (count) =>
            {
                //return tres[count];
                
                return new InstaDirectInboxItem();
            }, source.ThreadId);
            var Message = await AppCore.InstaApi.GetDirectInboxThreadAsync(source.ThreadId, PaginationParameters.MaxPagesToLoad(1));
            VieweIds = Message.Value.VieweId;
            ThreadIds = Message.Value.ThreadId;
            UserId = Message.Value.Users[0].Pk;
            DataUser = Message.Value.Users[0];
            //MessageList.ItemsSource = Message.Value.Items;
            Message.Value.Items.Reverse();
            //for (int a = 0; a < Message.Value.Items.Count; a++)
            //{
            //    MessageList.Items.Add(Message.Value.Items[a]);
            //}



            //foreach (InstaDirectInboxItem item in ItemsList)
            //{
            //    MessageList.Items.Insert(0, item);
            //}

            MessageList.ItemsSource = ItemsList;

            //var test = await AppCore.InstaApi.GetRecentRecipientsAsync();
        }
        void BackFunc()
        {
            MainView.HeaderD.Visibility = Visibility.Visible;
            Frame.GoBack();
        }
        private void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            BackFunc();
            AppCore.ModerateBack("");
        }

        private void TextBoxChat_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (TextBoxChat.Text.Length)
            {
                case 0:
                    DynamicIcon.Glyph = "\ueB9F";
                    return;
            }

            DynamicIcon.Glyph = "\ue122";
        }

        private async void DynamicButton_Click(object sender, RoutedEventArgs e)
        {
            switch (TextBoxChat.Text.Length)
            {
                case 0:

                    return;
            }


            var MessageSend = await AppCore.InstaApi.SendDirectMessage(UserId.ToString(), ThreadIds, TextBoxChat.Text);
            TextBoxChat.Text = "";
            for (int a = 0; a < MessageSend.Value[0].Items.Count; a++)
            {
                MessageList.Items.Add(MessageSend.Value[0].Items[a]);
            }


            //FOR TEST
            //var addItem = new InstaDirectInboxItem() { ItemType = InstaDirectThreadItemType.Text, Text = TextBoxChat.Text };
            //TextBoxChat.Text = "";
            //MessageList.Items.Add(addItem);

        }

        private void MessageList_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            //MessageList.Items.OrderBy(x => ((InstaDirectInboxItem)x).TimeStamp);
        }

       
    }
}
