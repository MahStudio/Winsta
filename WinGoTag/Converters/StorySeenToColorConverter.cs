using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WinGoTag.Converters
{
    class StorySeenToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.GetType() == typeof(InstaReelFeed))
            {
                var v = value as InstaReelFeed;
                if (v.Seen == 0)
                    return (SolidColorBrush)App.Current.Resources["SystemControlBackgroundAccentBrush"];
                else return new SolidColorBrush(Colors.Gray);
            }

            if (value.GetType() == typeof(InstaStory))
            {
                var v = value as InstaStory;
                if (v.SeenRankedPosition == 0)
                    return (SolidColorBrush)App.Current.Resources["SystemControlBackgroundAccentBrush"];
                else return new SolidColorBrush(Colors.Gray);
            }

            else return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
