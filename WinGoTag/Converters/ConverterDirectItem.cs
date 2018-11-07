using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using WinGoTag.View.DirectMessages;

namespace WinGoTag.Converters
{
    class ConverterDirectItem : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var med = (InstaDirectInboxItem)value;

                if (parameter.ToString() == "align")
                {
                    if (med.UserId == DirectChatView.UserId)
                        return HorizontalAlignment.Left;
                    else return HorizontalAlignment.Right;
                }

                if (parameter.ToString() == "color")
                {
                    if (med.UserId == DirectChatView.UserId)
                        return Colors.Transparent;
                    else return ((SolidColorBrush)Application.Current.Resources["AppBarBackground"]);
                }

                if (parameter.ToString() == "Avatar")
                {
                    if (med.UserId == DirectChatView.UserId)
                        return DirectChatView.DataUser;
                    else return null;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
