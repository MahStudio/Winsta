using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.EditProfile
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class EditProfile : Page
    {
        AccountUser UserInfo;
        public EditProfile()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(Frame.GoBack);
            DataContext = ((InstaUserInfo)e.Parameter);
            UserInfo = (await AppCore.InstaApi.AccountProcessor.GetRequestForEditProfileAsync()).Value.User;
        }

        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
            AppCore.ModerateBack("");
        }

        private async void EndBT_Click(object sender, RoutedEventArgs e)
        {
            //Gender (1 = male, 2 = female, 3 = unknown)
            GenderType G = GenderType.Unknown;
            switch (UserInfo.Gender)
            {
                case 1: G = GenderType.Male;break;
                case 2: G = GenderType.Female;break;
                default:
                    break;
            }
            var res = await AppCore.InstaApi.AccountProcessor.EditProfileAsync(txtExternalUrl.Text, UserInfo.PhoneNumber, txtFullName.Text, txtBiography.Text, UserInfo.Email, G, "");
            if(res.Succeeded)
            {
                Frame.GoBack();
                AppCore.ModerateBack("");
            }
            else { await new MessageDialog(res.Info.Message).ShowAsync(); }
        }

        private async void ChangePicture_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fop = new FileOpenPicker();
            fop.FileTypeFilter.Add(".jpg");
            fop.FileTypeFilter.Add(".jpeg");
            fop.ViewMode = PickerViewMode.Thumbnail;
            fop.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            var file = await fop.PickSingleFileAsync();
            if (file == null) return;
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(await file.OpenAsync(Windows.Storage.FileAccessMode.Read));
            PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
            var res = await AppCore.InstaApi.AccountProcessor.ChangeProfilePictureAsync(pixelData.DetachPixelData());
            if(res.Succeeded)
            {
                PPIB.ImageSource = new BitmapImage(new Uri(res.Value.User.ProfilePicUrl, UriKind.RelativeOrAbsolute));
            }
        }
    }
}
