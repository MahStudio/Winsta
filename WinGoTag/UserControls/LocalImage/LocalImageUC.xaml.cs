using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Controllo utente è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinGoTag.UserControls.LocalImage
{
    public sealed partial class LocalImageUC : UserControl
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string),
        typeof(LocalImageUC), new PropertyMetadata(null, SourceChanged));

        public LocalImageUC()
        {
            this.InitializeComponent();
        }
        public string Source
        {
            get { return this.GetValue(SourceProperty) as string; }
            set { this.SetValue(SourceProperty, value); }
        }

        private async static void SourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = (LocalImageUC)obj;

            var path = e.NewValue as string;

            if (string.IsNullOrEmpty(path))
            {
                control.Image.Source = null;
            }
            else
            {
                try
                {
                    var file = await StorageFile.GetFileFromPathAsync(path);

                    using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        await bitmapImage.SetSourceAsync(fileStream);
                        control.Image.Source = bitmapImage;

                    }
                }
                catch { }
                
            }
        }
    }
}