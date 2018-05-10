using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Search;
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
        public static PhotoGalleryView Current;

        private ImageFileInfo persistedItem;
        public ObservableCollection<ImageFileInfo> Images { get; } = new ObservableCollection<ImageFileInfo>();

        public event PropertyChangedEventHandler PropertyChanged;

        public PhotoGalleryView()
        {
            this.InitializeComponent();
            Current = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
          
            await GetItemsAsync();
        }


        public void UpdatePersistedItem(ImageFileInfo item)
        {
            persistedItem = item;
        }


        private async Task GetItemsAsync()

        {

            QueryOptions options = new QueryOptions();

            options.FolderDepth = FolderDepth.Deep;

            options.FileTypeFilter.Add(".jpg");

            options.FileTypeFilter.Add(".png");

            options.FileTypeFilter.Add(".gif");



            // Get the Pictures library

            Windows.Storage.StorageFolder picturesFolder = Windows.Storage.KnownFolders.CameraRoll;

            var result = picturesFolder.CreateFileQueryWithOptions(options);
            IReadOnlyList<StorageFile> imageFiles = await result.GetFilesAsync();
            bool unsupportedFilesFound = false;
        
            foreach (StorageFile file in imageFiles)
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


        //ContainerContentChanging="ImageGridView_ContainerContentChanging"
       



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

        private void Cover_ImageOpened(object sender)
        {
            DoubleAnimation fade = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3),
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
                //image.Opacity = 100;
                
                var item = args.Item as ImageFileInfo;

                try
                {
                    image.Source = await item.GetImageThumbnailAsync();

                    DoubleAnimation fade = new DoubleAnimation()
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(0.3),
                        EnableDependentAnimation = true
                    };
                    Storyboard.SetTarget(fade, image);
                    Storyboard.SetTargetProperty(fade, "Opacity");
                    Storyboard openpane = new Storyboard();
                    openpane.Children.Add(fade);
                    openpane.Begin();
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
        //private async Task<BitmapImage> GetImageStream(StorageFile image)
        //{
        //    StorageFile file = await StorageFile.GetFileFromPathAsync(image.Path);
        //    using (var stream = await file.OpenAsync(FileAccessMode.Read))
        //    {
        //        BitmapImage Img = new BitmapImage();
        //        Img.SetSource(stream);
        //        return Img;
        //    }
        //}


        private void Next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
