using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using static WinGoTag.View.AddPhotos.PhotoGalleryView;

namespace WinGoTag.Converters
{
    class ConvertStoragePath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (DesignMode.DesignModeEnabled)
                    return "images/VideoFileIcon.png";

                var fileName = (string)value;

                if (string.IsNullOrEmpty(fileName))
                    return "";


                return GetThumbnailImage(fileName);

            }
            catch
            {
                return "";
            }
        }


        private async Task<BitmapImage> GetThumbnailImage(string fileName)
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName)
                    .AsTask().ConfigureAwait(false);
                var thumbnail = await file.GetScaledImageAsThumbnailAsync(ThumbnailMode.ListView, 90, ThumbnailOptions.UseCurrentScale)
                    .AsTask().ConfigureAwait(false);
                var bmi = new BitmapImage();
                bmi.SetSource(thumbnail);
                return bmi;
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            return null;
        }




        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
