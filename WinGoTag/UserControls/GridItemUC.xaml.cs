using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Controllo utente è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinGoTag.UserControls
{
    public sealed partial class GridItemUC : UserControl, INotifyPropertyChanged
    {
        public InstaMedia Media
        {
            get
            {
                return (InstaMedia)GetValue(MediaProperty);
            }
            set
            {
                SetValue(MediaProperty, value);
                this.DataContext = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Media"));
            }
        }
        public static readonly DependencyProperty MediaProperty = DependencyProperty.Register(
         "Media",
         typeof(InstaMedia),
         typeof(GridItemUC),
         new PropertyMetadata(null)
        );

        public event PropertyChangedEventHandler PropertyChanged;
        public GridItemUC()
        {
            this.InitializeComponent();
            this.DataContextChanged += InstaMediaUC_DataContextChanged;
        }

        private void InstaMediaUC_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            try
            {
                if (args.NewValue.GetType() == typeof(InstaMedia))
                {
                    switch (Media.MediaType)
                    {
                        case InstaMediaType.Image:
                            SymbolType.Text = "";
                            Image.Source = new BitmapImage(new Uri(Media.Images.FirstOrDefault().URI, UriKind.RelativeOrAbsolute));
                            break;

                        case InstaMediaType.Carousel:
                            SymbolType.Text = "\ue923";
                            Image.Source = new BitmapImage(new Uri(Media.Carousel.FirstOrDefault().Images.FirstOrDefault().URI, UriKind.RelativeOrAbsolute));
                            break;
                         
                        case InstaMediaType.Video:
                            SymbolType.Text = "\ue714";
                            Image.Source = new BitmapImage(new Uri(Media.Images.FirstOrDefault().URI, UriKind.RelativeOrAbsolute));
                            break;
                    }
                    
                }
            }
            catch { }
        }
    }
}
