using InstaSharper.Classes.Models;
using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WinGoTag.DataBinding;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.AddPhotos
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class PhotoGalleryView : Page
    {
        //public static PhotoGalleryView Current;
        public class Folder
        {
            public string Name { get; set; }
            public StorageFolder folder { get; set; }
            //public int CountFiles { get; set; }
        }

        public List<Folder> ListFolders = new List<Folder>();

        public InstaImage instaImage = new InstaImage();

        private ImageFileInfo persistedItem;
        public ObservableCollection<ImageFileInfo> Images { get; } = new ObservableCollection<ImageFileInfo>();

        //public event PropertyChangedEventHandler PropertyChanged;
        public bool fullSize = false;
        public bool IsMultiSelect = false;

        public PhotoGalleryView()
        {
            this.InitializeComponent();
            //Current = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            IReadOnlyList<StorageFolder> folderList = await picturesFolder.GetFoldersAsync();

            foreach (StorageFolder folder in folderList)
            {
                ListFolder.Items.Add(new Folder { folder = folder, Name = folder.DisplayName});
            }

            ListFolder.SelectedIndex = 0;
        }


        private async void ListFolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((Folder)ListFolder.SelectedItem);
            await GetItemsAsync(item.folder);
            //ListImage.SelectedIndex = 0;
        }


        public void UpdatePersistedItem(ImageFileInfo item)
        {
            persistedItem = item;
        }


        private async Task GetItemsAsync(StorageFolder folder)
        {
            Images.Clear();

            QueryOptions options = new QueryOptions();
            options.FolderDepth = FolderDepth.Deep;
            options.FileTypeFilter.Add(".jpg");
            options.FileTypeFilter.Add(".png");
            options.FileTypeFilter.Add(".gif");
            options.FileTypeFilter.Add(".mp4");

            // Get the Pictures library

            //Windows.Storage.StorageFolder picturesFolder = Windows.Storage.KnownFolders.CameraRoll;

            var result = folder.CreateFileQueryWithOptions(options);
            IReadOnlyList<StorageFile> imageFiles = await result.GetFilesAsync();
            bool unsupportedFilesFound = false;
        
            foreach (StorageFile file in imageFiles.OrderBy(x=> x.DateCreated).Reverse())
            {
                // Only files on the local computer are supported. 
                // Files on OneDrive or a network location are excluded.
            
                if (file.Provider.Id == "computer")
                {
                    Images.Add(await LoadImageInfo(file));
                }
                else
                {
                    unsupportedFilesFound = true;
                }

                
            }

            if (unsupportedFilesFound == true)
            {
                ContentDialog unsupportedFilesDialog = new ContentDialog
                {
                    Title = "Unsupported images found",
                    Content = "This sample app only supports images stored locally on the computer. We found files in your library that are stored in OneDrive or another network location. We didn't load those images.",
                    CloseButtonText = "Ok"
                };

                ContentDialogResult resultNotUsed = await unsupportedFilesDialog.ShowAsync();
            }
        }
       



        private void ImageGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.InRecycleQueue)
            {
                var templateRoot = args.ItemContainer.ContentTemplateRoot as Grid;
                var image = (Image)templateRoot.FindName("ItemImage");
                image.Source = null;
            }
            if (args.Phase == 0)
            {
                args.RegisterUpdateCallback(ShowImage);
                args.Handled = true;
            }
        }


        private void AnimationImage(object sender)
        {
            DoubleAnimation fade = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(fade, (Image)sender);
            Storyboard.SetTargetProperty(fade, "Opacity");
            Storyboard openpane = new Storyboard();
            openpane.Children.Add(fade);
            openpane.Begin();
        }


        private async void ShowImage(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
        
            if (args.Phase == 1)
            {
                // It's phase 1, so show this item's image.
                var templateRoot = args.ItemContainer.ContentTemplateRoot as Grid;
                var image = (Image)templateRoot.FindName("ItemImage");
                
                var item = args.Item as ImageFileInfo;
                try
                {
                    image.Source = await item.GetImageThumbnailAsync();
                    AnimationImage(image);
                }
            
                catch (Exception)
                {
                    // File could be corrupt, or it might have an image file
                    // extension, but not really be an image file.
                    BitmapImage bitmapImage = new BitmapImage();

                    bitmapImage.UriSource = new Uri(image.BaseUri, "Assets/StoreLogo.png");
                    image.Source = bitmapImage;
                }
            }

        }

        public async static Task<ImageFileInfo> LoadImageInfo(StorageFile file)
        {
            var properties = await file.Properties.GetImagePropertiesAsync();
            ImageFileInfo info = new ImageFileInfo(
                properties, file,
                file.DisplayName, file.DisplayType);
            return info;
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            var item = ListImage.SelectedItem as ImageFileInfo;
            instaImage = new InstaImage
            {
                Height = 1080,
                Width = 1080,
                URI = new Uri(item.ImageFile.Path, UriKind.Absolute).LocalPath
            };

            //await upload();
        }

        public async Task Upload()
        {
            var UploadTest = await AppCore.InstaApi.UploadPhotoAsync(instaImage, "");
        }

        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void ListImage_ItemClick(object sender, ItemClickEventArgs e)
        {
            //var item = e.ClickedItem as ImageFileInfo;
            //PreviewPictures.DataContext = item;
            //PreviewPictures.Source = await item.GetImageThumbnailAsync();

        }

        private async void PreviewPictures_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            try
            {
                var item = args.NewValue as ImageFileInfo;
                PreviewPictures.Source = await item.GetImageThumbnailAsync();
            }
            catch { }
        }

        private void ListImage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ListImage.SelectedItem as ImageFileInfo;
            PreviewPictures.DataContext = item;
        }

        private void ReSize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = PreviewPictures.DataContext as ImageFileInfo;
                if (fullSize is false)
                {
                    var zoomFactor = (float)Math.Min(MainImageScroller.ActualWidth / item.ImageProperties.Width,
                    MainImageScroller.ActualHeight / item.ImageProperties.Height);
                    MainImageScroller.ChangeView(null, null, zoomFactor);

                    fullSize = true; return;
                }

                if (fullSize is true)
                {
                    MainImageScroller.ChangeView(null, null, 1); fullSize = false;
                }
            }
            catch(Exception x)
            {
                
            }
        }

        private void MultiSelect_Click(object sender, RoutedEventArgs e)
        {
            if(IsMultiSelect is false)
            {
                ListImage.SelectionMode = ListViewSelectionMode.Multiple; IsMultiSelect = true; return;
            }
             if(IsMultiSelect is true)
            {
                ListImage.SelectionMode = ListViewSelectionMode.Single; IsMultiSelect = false;
            }
        }
    }
}
