using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WinGoTag.Converters
{
    class LikeButtonColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter.ToString() == "foreground")
                if (System.Convert.ToBoolean(value))
                    return new SolidColorBrush(Colors.DarkRed);
                else return new SolidColorBrush(Colors.Black);
            else return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
