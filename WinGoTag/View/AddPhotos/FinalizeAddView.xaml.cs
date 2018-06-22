using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.AddPhotos
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FinalizeAddView : Page
    {
        Uri ImageFileUri { get; set; }
        public FinalizeAddView() => this.InitializeComponent();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(Frame.GoBack);
            ImageFileUri = e.Parameter as Uri;
            var bmp = new BitmapImage(ImageFileUri);
            SFImage.Source = bmp;
        }

        private void ToBackBT_Click(object sender, RoutedEventArgs e) => AppCore.ModerateBack(Frame.GoBack);

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
                AppCore.ModerateBack("");
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            var sf = await StorageFile.GetFileFromApplicationUriAsync(ImageFileUri);
            var size = await sf.Properties.GetImagePropertiesAsync();

            var v = await (AppCore.InstaApi.UploadPhotoAsync(
                    new InstaSharper.Classes.Models.InstaImage(ImageFileUri.LocalPath, (int)size.Width, (int)size.Height), txtcaption.Text));
            if(v.Succeeded)
            {
                await new MessageDialog("Your photo uploaded successfully").ShowAsync();
            }
        }
    }
}
