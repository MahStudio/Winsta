using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WinGoTag.Converters
{
    class ConverterGridItemSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var med = (InstaMedia)value;
                switch (med.MediaType)
                {
                    case InstaMediaType.Image:
                        
                        return new Uri(med.Images.FirstOrDefault().URI, UriKind.RelativeOrAbsolute);


                    case InstaMediaType.Carousel:
                   
                        return new Uri(med.Carousel.FirstOrDefault().Images.FirstOrDefault().URI, UriKind.RelativeOrAbsolute);
                      

                    case InstaMediaType.Video:
             
                        return new Uri(med.Images.FirstOrDefault().URI, UriKind.RelativeOrAbsolute);
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
