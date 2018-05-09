using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.AddPhotos
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class PhotoGalleryView : Page
    {
        public List<Picures> PicuresList = new List<Picures>();
        
        private string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        public class Picures
        {
            public BitmapImage Bitmap { get; set; }
        }

        public PhotoGalleryView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AppCore.ModerateBack(Frame.GoBack);
            GetCameraRoll();
        }


        private async void GetCameraRoll()
        {
            var images = await KnownFolders.CameraRoll.GetFilesAsync();

            //foreach (var image in images)
            //{
            //var y = images[0].GetScaledImageAsThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.PicturesView);
            //}

            for (int a = 0; a < images.Count; a++)
            {
                if(a == 35) { break; }
               
                Picures bit = new Picures() { Bitmap= await GetImageStream(images[a])};
               
                ListImage.Items.Add(bit);

            }

        }



        private async Task<BitmapImage> GetImageStream(StorageFile image)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(image.Path);
            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapImage Img = new BitmapImage();
                Img.SetSource(stream);
                return Img;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
