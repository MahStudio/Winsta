using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WinGoTag.Converters
{
    class UtcToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var dt = System.Convert.ToDateTime(value);
                var sub = DateTime.UtcNow.Subtract(dt);
                if (sub.TotalDays >= 7)
                    return $"{System.Convert.ToInt32(sub.TotalDays / 7)} w";
                if (sub.TotalDays >= 1)
                    return $"{System.Convert.ToInt32(sub.TotalDays)} d";
                if (sub.TotalHours >= 1)
                    return $"{System.Convert.ToInt32(sub.TotalHours)} h";
                if (sub.TotalMinutes >= 1)
                    return $"{System.Convert.ToInt32(sub.TotalMinutes)} m";
                return "Recently";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
