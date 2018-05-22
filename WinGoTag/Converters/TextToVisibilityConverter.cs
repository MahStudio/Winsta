using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinGoTag.Converters
{
    class TextToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is "")
            {
                return Visibility.Collapsed;
            }


            if (value is 0)
            {
                return Visibility.Collapsed;
            }


            if (value is null)
            {
                return Visibility.Collapsed;
            }

            else return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
