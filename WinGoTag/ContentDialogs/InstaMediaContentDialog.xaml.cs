using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.ContentDialogs
{
    public sealed partial class InstaMediaContentDialog : ContentDialog
    {
        InstaMedia _med;
        private class LVItem
        {
            public string Text { get; set; }
            public string Tag { get; set; }
        }
        public InstaMediaContentDialog(InstaMedia Media)
        {
            this.InitializeComponent();
            this.DataContext = Media;
            _med = Media;
            if (!Media.User.IsPrivate)
            {
                Commands.Items.Add(new LVItem { Text = "Copy URL", Tag = "Copy" });
            }
            Commands.Items.Add(new LVItem { Text = "Cancel", Tag = "Cancel" });
        }

        #region ContentDialog Default buttons events
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
        #endregion

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch ((e.ClickedItem as LVItem).Tag)
            {
                case "Copy":
                    var dp = new DataPackage();
                    dp.SetText("https://instagram.com/p/" + _med.Code);
                    Clipboard.SetContent(dp);
                    Hide();
                    break;
                case "Cancel":
                    Hide();
                    break;
                default:
                    break;
            }
        }
    }
}
