using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        public DirectChatView() => InitializeComponent();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(BackFunc);
            DataContext = ((InstaDirectInboxThread)e.Parameter);
            var source = ((InstaDirectInboxThread)e.Parameter);
            ItemsList = new GenerateDirectThreadList<InstaDirectInboxItem>(100000, (count) => new InstaDirectInboxItem()
            , source.ThreadId);
            var message = await AppCore.InstaApi.GetDirectInboxThreadAsync(source.ThreadId, PaginationParameters.MaxPagesToLoad(1));
            VieweIds = message.Value.VieweId;
            ThreadIds = message.Value.ThreadId;
            DataUser = message.Value.Users[0];
            try
            {
                UserId = message.Value.Users[0].Pk;
            }
            catch
            {
                UserId = (await AppCore.InstaApi.GetCurrentUserAsync()).Value.Pk;
            }

            // MessageList.ItemsSource = Message.Value.Items;
            message.Value.Items.Reverse();
            // for (int a = 0; a < Message.Value.Items.Count; a++)
            // {
            //    MessageList.Items.Add(Message.Value.Items[a]);
            // }

            // foreach (InstaDirectInboxItem item in ItemsList)
            // {
            //    MessageList.Items.Insert(0, item);
            // }

            MessageList.ItemsSource = ItemsList;

            // var test = await AppCore.InstaApi.GetRecentRecipientsAsync();
        }

        void BackFunc()
        {
            MainView.HeaderD.Visibility = Visibility.Visible;
            Frame.GoBack();
        }

        void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            BackFunc();
            AppCore.ModerateBack("");
        }

        void TextBoxChat_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (TextBoxChat.Text.Length)
            {
                case 0:
                    DynamicIcon.Glyph = "\ueB9F";
                    return;
            }

            DynamicIcon.Glyph = "\ue122";
        }

        async void DynamicButton_Click(object sender, RoutedEventArgs e)
        {
            switch (TextBoxChat.Text.Length)
            {
                case 0:
                    return;
            }

            var MessageSend = await AppCore.InstaApi.SendDirectMessage(UserId.ToString(), ThreadIds, TextBoxChat.Text);
            TextBoxChat.Text = "";
            for (int a = 0; a < MessageSend.Value[0].Items.Count; a++)
                ItemsList.Add(MessageSend.Value[0].Items[a]);
            // MessageList.Items.Add(MessageSend.Value[0].Items[a]);


            // FOR TEST
            // var addItem = new InstaDirectInboxItem() { ItemType = InstaDirectThreadItemType.Text, Text = TextBoxChat.Text };
            // TextBoxChat.Text = "";
            // MessageList.Items.Add(addItem);
        }

        void MessageList_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            // MessageList.Items.OrderBy(x => ((InstaDirectInboxItem)x).TimeStamp);
        }
    }
}