using InstaNotifications;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
#if !DEBUG
            if (!Media.User.IsPrivate || Media.PhotoOfYou || Media.User.UserName == AppCore.InstaApi.GetLoggedUser().UserName)
            {
#endif
                Commands.Items.Add(new LVItem { Text = "Copy URL", Tag = "Copy" });
                Commands.Items.Add(new LVItem { Text = "Download content", Tag = "Download" });
#if !DEBUG
            }
#endif
            Commands.Items.Add(new LVItem { Text = "Copy caption", Tag = "CC" });
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
                case "CC":
                    var dp = new DataPackage();
                    dp.SetText(_med.Caption.Text);
                    Clipboard.SetContent(dp);
                    Hide();
                    break;
                case "Download":
                    DownloadMediaContent();
                    Hide();
                    break;
                case "Copy":
                    var dp2 = new DataPackage();
                    dp2.SetText("https://instagram.com/p/" + _med.Code);
                    Clipboard.SetContent(dp2);
                    Hide();
                    break;
                case "Cancel":
                    Hide();
                    break;
                default:
                    break;
            }
        }

        private async void DownloadMediaContent()
        {
            var dt = DateTime.Now;
            var f = await DownloadsFolder.CreateFolderAsync((dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + "-" + dt.Hour + "-" + dt.Minute), CreationCollisionOption.GenerateUniqueName);
            List<StorageFile> Files = new List<StorageFile>();
            switch (_med.MediaType)
            {
                case InstaMediaType.Image:
                    Files.Add(await f.CreateFileAsync("Img.jpg", CreationCollisionOption.GenerateUniqueName));
                    var file = Files.FirstOrDefault();
                    var tsk = new Windows.Networking.BackgroundTransfer.BackgroundDownloader().CreateDownload(new Uri(_med.Images[0].URI, UriKind.RelativeOrAbsolute), file).StartAsync().AsTask();
                    NotifyHelper.CreateNotifyEmpty("Download successfull", $"Image saved into {file.Path}");
                    break;
                case InstaMediaType.Video:
                    Files.Add(await f.CreateFileAsync("Video.mp4", CreationCollisionOption.GenerateUniqueName));
                    var file2 = Files.FirstOrDefault();
                    var tsk2 = new Windows.Networking.BackgroundTransfer.BackgroundDownloader().CreateDownload(new Uri(_med.Videos[0].Url, UriKind.RelativeOrAbsolute), file2).StartAsync().AsTask();
                    NotifyHelper.CreateNotifyEmpty("Download successfull", $"Image saved into {file2.Path}");
                    break;
                case InstaMediaType.Carousel:
                    foreach (var item in _med.Carousel)
                    {
                        if (item.MediaType == InstaMediaType.Image)
                        {
                            var file3 = await f.CreateFileAsync("Img.jpg", CreationCollisionOption.GenerateUniqueName);
                            var tsk3 = new Windows.Networking.BackgroundTransfer.BackgroundDownloader().CreateDownload(new Uri(item.Images[0].URI, UriKind.RelativeOrAbsolute), file3).StartAsync().AsTask();
                            NotifyHelper.CreateNotifyEmpty("Download successfull", $"Image saved into {file3.Path}");
                        }
                        else
                        {
                            var file3 = await f.CreateFileAsync("Video.mp4", CreationCollisionOption.GenerateUniqueName);
                            var tsk3 = new Windows.Networking.BackgroundTransfer.BackgroundDownloader().CreateDownload(new Uri(item.Videos[0].Url, UriKind.RelativeOrAbsolute), file3).StartAsync().AsTask();
                            NotifyHelper.CreateNotifyEmpty("Download successfull", $"Video saved into {file3.Path}");
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
