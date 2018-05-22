using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WinGoTag.Converters
{
    class UtcToTextConverterMedia : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dt = System.Convert.ToDateTime(value);
            var sub = DateTime.UtcNow.Subtract(dt);
            if (sub.TotalDays >= 7)
                return $"{System.Convert.ToInt32(sub.TotalDays / 7)} WEEKS AGO";
            if (sub.TotalDays >= 1)
                return $"{System.Convert.ToInt32(sub.TotalDays)} DAYS AGO";
            if (sub.TotalHours >= 1)
                return $"{System.Convert.ToInt32(sub.TotalHours)} HOURS AGO";
            if (sub.TotalMinutes >= 1)
                return $"{System.Convert.ToInt32(sub.TotalMinutes)} MINUTES AGO";
            return "Recently";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
