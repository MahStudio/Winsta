using InstaSharper.Classes.Models;
using System;
using System.Collections.Specialized;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using WinGoTag.DataBinding;
using WinGoTag.Helpers;
using WinGoTag.View.UserViews;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CommentsView : Page
    {
        string MediaID = "";
        public GenerateComments<InstaComment> PageItemssource;
        public CommentsView() => InitializeComponent();
        void MessageTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                SendTextMessage();
        }

        async void SendTextMessage()
        {
            var res = await AppCore.InstaApi.CommentMediaAsync(MediaID, MessageTextBox.Text);
            if (!res.Succeeded)
            {
                await new MessageDialog(res.Info.Message).ShowAsync();
                return;
            }
            else
            {
                PageItemssource.Add(res.Value);
                MessageTextBox.Text = "";
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(Frame.GoBack);
            MediaID = e.Parameter.ToString();
            if (PageItemssource != null)
                PageItemssource.CollectionChanged -= PageItemssource_CollectionChanged;

            PageItemssource = new GenerateComments<InstaComment>(100000, (count) => new InstaComment(), e.Parameter.ToString());
            PageItemssource.CollectionChanged += PageItemssource_CollectionChanged;
            mylist.ItemsSource = PageItemssource;
        }

        void PageItemssource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
            AppCore.ModerateBack("");
        }

        void Username_Click(object sender, RoutedEventArgs e) => Frame.Navigate(typeof(UserProfileView), (sender as HyperlinkButton).Tag);

        void Reply_Button(object sender, RoutedEventArgs e) => MessageTextBox.Text = $"@{((sender as HyperlinkButton).Tag as InstaComment).User.UserName}";

        void RichTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                if (sender is RichTextBlock richText && richText != null)
                {
                    try
                    {
                        if (richText.DataContext is InstaComment comment && comment != null)
                            using (var pg = new PassageHelper())
                            {
                                var passages = pg.GetParagraph(comment.Text, HyperLinkClick);
                                richText.Blocks.Clear();
                                richText.Blocks.Add(passages);
                            }

                    }
                    catch { }
                }
            }
        }

        void HyperLinkClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            if (sender == null)
                return;
            try
            {
                if (sender.Inlines.Count > 0)
                    if (sender.Inlines[0] is Run run && run != null)
                    {
                        var text = run.Text;
                        text = text.ToLower();
                        run.Text.ShowInOutput();
                        if (text.StartsWith("http://") || text.StartsWith("https://") || text.StartsWith("www."))
                            run.Text.OpenUrl();
                        else
                        {
                            ToBackBT_Click(ToBackBT, null);
                            MainPage.Current?.PushSearch(text);
                        }
                    }
            }
            catch (Exception ex) { ex.ExceptionMessage("CommentsView.HyperLinkClick"); }
        }
        
    }
}