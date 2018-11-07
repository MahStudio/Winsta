using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WinGoTag.Converters
{
    class CarouselSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = (InstaMedia)value;
            int maxint = 0;
            if (v.MediaType == InstaMediaType.Carousel)
            {
                foreach (var item in v.Carousel)
                {
                    if (item.MediaType == InstaMediaType.Image)
                    {
                        if (parameter.ToString() == "w")
                        {
                            if (item.Images[0].Width > maxint) maxint = item.Images[0].Width;
                        }
                        else if (parameter.ToString() == "h")
                        {
                            if (item.Images[0].Height > maxint) maxint = item.Images[0].Height;
                        }
                        else return 0;
                    }
                    else if (item.MediaType == InstaMediaType.Video)
                    {
                        if (parameter.ToString() == "w")
                        {
                            if (item.Videos[0].Width > maxint) maxint = item.Videos[0].Width;
                        }
                        else if (parameter.ToString() == "h")
                        {
                            if (item.Videos[0].Height > maxint) maxint = item.Videos[0].Height;
                        }
                        else return 0;
                    }
                    else return 0;
                }
                return maxint;
            }
            else return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
