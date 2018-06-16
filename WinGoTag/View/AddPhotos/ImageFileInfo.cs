using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace WinGoTag.View.AddPhotos
{
    public class ImageFileInfo : INotifyPropertyChanged
    {
        public ImageFileInfo(ImageProperties properties, StorageFile imageFile, string name, string type)
        {
            ImageProperties = properties;
            // ImageName = name;
            // ImageFileType = type;
            ImageFile = imageFile;
            // var rating = (int)properties.Rating;
            // var random = new Random();
            // ImageRating = rating == 0 ? random.Next(1, 5) : rating;
        }

        public StorageFile ImageFile { get; }

        public ImageProperties ImageProperties { get; }

        public async Task<BitmapImage> GetImageSourceAsync()
        {
            using (IRandomAccessStream fileStream = await ImageFile.OpenReadAsync())
            {
                // Create a bitmap to be the image source.
                var bitmapImage = new BitmapImage();
                bitmapImage.SetSource(fileStream);

                return bitmapImage;
            }
        }

        public async Task<BitmapImage> GetImageThumbnailAsync()
        {
            var thumbnail = await ImageFile.GetThumbnailAsync(ThumbnailMode.PicturesView);
            // Create a bitmap to be the image source.
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(thumbnail);
            thumbnail.Dispose();

            return bitmapImage;
        }

        public string ImageName { get; }

        public string ImageFileType { get; }

        public string ImageDimensions => $"{ImageProperties.Width} x {ImageProperties.Height}";

        public string ImageTitle
        {
            get => string.IsNullOrEmpty(ImageProperties.Title) ? ImageName : ImageProperties.Title;
            set
            {
                if (ImageProperties.Title != value)
                {
                    ImageProperties.Title = value;
                    var ignoreResult = ImageProperties.SavePropertiesAsync();
                    OnPropertyChanged();
                }
            }
        }

        public int ImageRating
        {
            get => (int)ImageProperties.Rating;
            set
            {
                if (ImageProperties.Rating != value)
                {
                    ImageProperties.Rating = (uint)value;
                    var ignoreResult = ImageProperties.SavePropertiesAsync();
                    OnPropertyChanged();
                }
            }
        }

        float exposure = 0;
        public float Exposure
        {
            get => exposure;
            set => SetEditingProperty(ref exposure, value);
        }

        float temperature = 0;
        public float Temperature
        {
            get => temperature;
            set => SetEditingProperty(ref temperature, value);
        }

        float tint = 0;
        public float Tint
        {
            get => tint;
            set => SetEditingProperty(ref tint, value);
        }

        float contrast = 0;
        public float Contrast
        {
            get => contrast;
            set => SetEditingProperty(ref contrast, value);
        }

        float saturation = 1;
        public float Saturation
        {
            get => saturation;
            set => SetEditingProperty(ref saturation, value);
        }

        float blur = 0;
        public float Blur
        {
            get => blur;
            set => SetEditingProperty(ref blur, value);
        }

        bool needsSaved;
        public bool NeedsSaved
        {
            get => needsSaved;
            set => SetProperty(ref needsSaved, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetEditingProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (SetProperty(ref storage, value, propertyName))
            {
                if (Exposure != 0 || Temperature != 0 || Tint != 0 || Contrast != 0 || Saturation != 1 || Blur != 0)
                {
                    NeedsSaved = true;
                }
                else
                {
                    NeedsSaved = false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }
            else
            {
                storage = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
    }
}