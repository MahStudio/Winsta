using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinGoTag.Converters
{
    class ConvertPivotTab : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var currentViewState = Window.Current.Bounds.Width;
                
                var W = currentViewState / 4;

                return W - 24;
            }
            catch
            {
                return 70;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
