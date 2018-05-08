using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using WinGoTag.View.DirectMessages;

namespace WinGoTag.Converters
{
    class DirectMessageControlVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var med = (InstaDirectInboxItem)value;
                if (med.Media != null)
                {
                    if (parameter.ToString() == "2")
                        return Visibility.Visible;
                    else return Visibility.Collapsed;
                }

                if (med.MediaShare != null)
                {
                    if (parameter.ToString() == "2")
                        return Visibility.Visible;
                    else return Visibility.Collapsed;
                }

                if (med.Text != null)
                {
                    if (parameter.ToString() == "1")
                        return Visibility.Visible;
                    else return Visibility.Collapsed;
                }

                if (med.UserId == DirectChatView.UserId)
                {
                    //UserAvatar.DataContext = DirectChatView.DataUser;
                    //SampleText.Background = null;
                    //Post.Background = null;
                    return HorizontalAlignment.Left;
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
