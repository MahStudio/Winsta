using InstaSharper.Classes.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinGoTag.Converters
{
    class InstaMediaControlVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var med = (InstaMedia)value;
                if (med.Videos.Count > 0)
                {
                    if (parameter.ToString() == "2")
                        return Visibility.Visible;
                    else return Visibility.Collapsed;
                }
                if (med.Images.Count > 0)
                {
                    if (parameter.ToString() == "1")
                        return Visibility.Visible;
                    else return Visibility.Collapsed;
                }
                if(med.Carousel.Count>0)
                {
                    if (parameter.ToString() == "3")
                        return Visibility.Visible;
                    else return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
