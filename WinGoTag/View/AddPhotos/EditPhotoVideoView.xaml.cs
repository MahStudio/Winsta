using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using Lumia.Imaging.Artistic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WinGoTag.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.AddPhotos
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditPhotoVideoView : Page
    {
        enum ImageFiltersEnum
        {
            NoFilter,
            Antique,
            BigNose,
            Cartoon,
            ColorSketch,
            Fog,
            Grayscale,
            GraySketch,
            LomoRed,
            LomoBlue,
            LomoGreen,
            LomoYellow,
            MagicPen,
            Milky,
            Mirrror,
            MoonLight,
            Negative,
            Oily,
            Paint,
            Posterize,
            Sepia,
            Solarize,
            SpotlightEffect,
            SqureBlur
        }
        IImageProvider LastEffect;
        StorageFile imageStorageFile;
        class FilterListItem
        {
            public Uri bitmapSource { get; set; }
            public string FilterName { get; set; }
        }
        int EffectIndex = -1;
        public EditPhotoVideoView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
                AppCore.ModerateBack("");
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
                AppCore.ModerateBack(Frame.GoBack);
            imageStorageFile = e.Parameter as StorageFile;
            // bitmapImage = await ((ImageFileInfo)e.Parameter).GetImageSourceAsync();
            await Task.Delay(1000);

            // EditItem.AddEditItem vm = new EditItem.AddEditItem();
            // this.DataContext = vm;
            // EditsList.ItemsSource = vm.GridViewItems;
            foreach (var item in Enum.GetNames(typeof(ImageFiltersEnum)))
            {
                FiltersList.Items.Add(new FilterListItem() { bitmapSource = await AddFilter((ImageFiltersEnum)Enum.Parse(typeof(ImageFiltersEnum), item)), FilterName = item });
            }
            // (new FilterListItem()).bitmapSource.Source
            // Vibrance(100);
            Preview_Image.Source = new BitmapImage(await NoFilter());
        }

        async Task<Uri> AddFilter(ImageFiltersEnum FilterType)
        {
            switch (FilterType)
            {
                case ImageFiltersEnum.NoFilter:
                    return await NoFilter();
                case ImageFiltersEnum.Antique:
                    return await Antique();
                case ImageFiltersEnum.BigNose:
                    return await BigNose();
                case ImageFiltersEnum.Cartoon:
                    return await Cartoon();
                case ImageFiltersEnum.ColorSketch:
                    return await ColorSketch();
                case ImageFiltersEnum.Fog:
                    return await Fog();
                case ImageFiltersEnum.Grayscale:
                    return await Grayscale();
                case ImageFiltersEnum.GraySketch:
                    return await GraySketch();
                case ImageFiltersEnum.LomoRed:
                    return await LomoRed();
                case ImageFiltersEnum.LomoBlue:
                    return await LomoBlue();
                case ImageFiltersEnum.LomoGreen:
                    return await LomoGreen();
                case ImageFiltersEnum.LomoYellow:
                    return await LomoYellow();
                case ImageFiltersEnum.MagicPen:
                    return await MagicPen();
                case ImageFiltersEnum.Milky:
                    return await Milky();
                case ImageFiltersEnum.Mirrror:
                    return await Mirrror();
                case ImageFiltersEnum.MoonLight:
                    return await Moonlight();
                case ImageFiltersEnum.Negative:
                    return await Negative();
                case ImageFiltersEnum.Oily:
                    return await Oil();
                case ImageFiltersEnum.Paint:
                    return await Paint();
                case ImageFiltersEnum.Posterize:
                    return await Posterize();
                case ImageFiltersEnum.Sepia:
                    return await Sepia();
                case ImageFiltersEnum.Solarize:
                    return await Solarize();
                case ImageFiltersEnum.SpotlightEffect:
                    return await SpotlightEffect();
                case ImageFiltersEnum.SqureBlur:
                    return await SqureBlur(40);
                default:
                    return await NoFilter();
            }
            // using (var source = new StorageFileImageSource(imageStorageFile))
            // using (var sharpnessEffect = new BlurEffect(source) { KernelSize = 40 })
            // using (var renderer = new JpegRenderer(sharpnessEffect))
            // {
            //    return await SaveToImage();
            // }
        }

        async Task<Uri> NoFilter()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                LastEffect = source;
                return await SaveToImage();
            }
        }

        async Task<Uri> Antique()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new AntiqueEffect(source))
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Cartoon()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new CartoonEffect(source) { DistinctEdges = false })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Emboss()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new EmbossEffect(source) { Level = 0.5 })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Fog()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new FogEffect(source))
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> GrayscaleNegative()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new GrayscaleNegativeEffect(source) { })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> LomoRed()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new LomoEffect(source) { LomoStyle = LomoStyle.Red })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> LomoBlue()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new LomoEffect(source) { LomoStyle = LomoStyle.Blue })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> LomoGreen()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new Lumia.Imaging.Artistic.LomoEffect(source) { LomoStyle = LomoStyle.Green })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> LomoYellow()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new LomoEffect(source) { LomoStyle = LomoStyle.Yellow })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> MagicPen()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new Lumia.Imaging.Artistic.MagicPenEffect(source) { })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Milky()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new MilkyEffect(source) { })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Mirrror()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new Lumia.Imaging.Artistic.MirrorEffect(source) { })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Moonlight()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new MoonlightEffect(source) { })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Negative()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new Lumia.Imaging.Artistic.NegativeEffect(source) { })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Oil()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new OilyEffect(source) { OilBrushSize = OilBrushSize.Medium })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Paint()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new PaintEffect(source) { Level = 4 })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Posterize()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new PosterizeEffect(source) { ColorComponentValueCount = 10 })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Sepia()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new Lumia.Imaging.Artistic.SepiaEffect(source) { })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> ColorSketch()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new SketchEffect(source) { SketchMode = SketchMode.Color })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> GraySketch()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new SketchEffect(source) { SketchMode = SketchMode.Gray })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Solarize()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new SolarizeEffect(source) { })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> BigNose()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new WarpingEffect(source) { WarpMode = WarpMode.BigNose })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> SpotlightEffect()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new SpotlightEffect(source) { Position = new Point((inf.ImageSize.Width / 2), (inf.ImageSize.Height / 2)), Radius = (int)((inf.ImageSize.Width / 2) - 100), TransitionSize = 0.8 })
                {
                    LastEffect = sharpnessEffect;
                    return await SaveToImage();
                }
            }
        }

        async Task<Uri> Brightness(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new BrightnessEffect(contrastEffect) { Level = (EffectPercentage / 100) })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> ColorAdjust(int RedPercentage, int GreenPercentage, int BluePercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new ColorAdjustEffect(contrastEffect) { Blue = (BluePercentage / 100), Green = (GreenPercentage / 100), Red = (RedPercentage / 100) })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> ColorBoost(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new ColorBoostEffect(contrastEffect) { Gain = (EffectPercentage / 50) })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> Contrast(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var sharpnessEffect = new ContrastEffect(source) { Level = (EffectPercentage / 100) })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> Despeckle(int EffectPercentage)
        {
            var Level = DespeckleLevel.Minimum;
            if (EffectPercentage < 25) Level = DespeckleLevel.Minimum;
            if (EffectPercentage > 25 && EffectPercentage <= 50) Level = DespeckleLevel.Low;
            if (EffectPercentage > 50 && EffectPercentage <= 75) Level = DespeckleLevel.High;
            if (EffectPercentage > 75) Level = DespeckleLevel.Maximum;
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new DespeckleEffect(contrastEffect) { DespeckleLevel = Level })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> Exposure(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new ExposureEffect(contrastEffect) { Gain = (EffectPercentage / 67), ExposureMode = ExposureMode.Natural })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> GaussianNoise(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new GaussianNoiseEffect(contrastEffect) { Level = (EffectPercentage / 100) })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> Grayscale()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var sharpnessEffect = new GrayscaleEffect(source))
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> LocalBoost(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new LocalBoostAutomaticEffect(contrastEffect) { Level = (EffectPercentage / 100) })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> Noise(int EffectPercentage)
        {
            var level = NoiseLevel.Minimum;
            if (EffectPercentage <= 35) level = NoiseLevel.Minimum;
            else if (EffectPercentage > 70) level = NoiseLevel.Maximum;
            else level = NoiseLevel.Medium;
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new NoiseEffect(contrastEffect) { Level = level })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> Sharpness(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new SharpnessEffect(contrastEffect) { Level = (EffectPercentage / 100) })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> Temperature(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new TemperatureAndTintEffect(contrastEffect) { Temperature = (EffectPercentage / 100) })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> Vibrance(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new VibranceEffect(contrastEffect) { Level = (EffectPercentage / 100) })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        async Task<Uri> SqureBlur(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new BlurEffect(contrastEffect) { KernelSize = EffectPercentage })
            {
                LastEffect = sharpnessEffect;
                return await SaveToImage();
            }
        }

        void FiltersList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as FilterListItem;
            FiltersList.SelectedItem = e.ClickedItem;
            Preview_Image.Source = new BitmapImage(item.bitmapSource);
        }

        void ToBackBT_Click(object sender, RoutedEventArgs e) => Frame.GoBack();

        // private void SliderContrast_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        // {
        //    if (EffectIndex is -1) { return; }

        //    int value = Convert.ToInt32(SliderContrast.Value);
        //    if (value is 0) { NoFilter(); return; }

        //    switch (EffectIndex)
        //    {
        //        case 0: /*add*/ break;
        //        case 1: Brightness(value); break;
        //        case 2: Contrast(value); break;
        //        case 3: Temperature(value); break;
        //        case 4: ColorBoost(value); break;
        //        case 5: /*Add*/ break;
        //        case 6: /*add*/ break;
        //    }
        // }

        // private void EditsList_ItemClick(object sender, ItemClickEventArgs e)
        // {
        //    var data = e.ClickedItem as GridViewEditItem;
        //    EffectIndex = data.Target;
        //    _NameEffect.Text = data.Text;
        //    EditRoot.Visibility = Visibility.Visible;
        // }

        // private void Cancel_Click(object sender, RoutedEventArgs e)
        // {
        //    EditRoot.Visibility = Visibility.Collapsed;
        // }

        // private void Done_Click(object sender, RoutedEventArgs e)
        // {
        //    EditRoot.Visibility = Visibility.Collapsed;
        // }

        async Task<Uri> SaveToImage()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var renderer = new JpegRenderer(LastEffect, JpegOutputColorMode.Yuv420))
            {
                var info = await source.GetInfoAsync();
                var R = AspectRatioHelper.Aspect(Convert.ToInt32(info.ImageSize.Width), Convert.ToInt32(info.ImageSize.Height));
                if (!SupportedAspectRatio(R))
                {
                    var max = Math.Max(info.ImageSize.Height, info.ImageSize.Width);
                    renderer.Size = new Size(max, max);
                }
                var saveAsTarget = await ApplicationData.Current.LocalFolder.CreateFileAsync("file.Jpg", CreationCollisionOption.GenerateUniqueName);
                var render = await renderer.RenderAsync();
                using (var fs = await saveAsTarget.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await fs.WriteAsync(render);
                    await fs.FlushAsync();
                    return new Uri($"ms-appdata:///local/{saveAsTarget.Name}", UriKind.RelativeOrAbsolute);
                }
            }
        }

        async void Next_Click(object sender, RoutedEventArgs e)
        {
            // Width of Photo should be 1080 at last
            // photo that has a width between 320 and 1080 pixels,
            // photo's aspect ratio is between 1.91:1 and 4:5 (a height between 566 and 1350 pixels with a width of 1080 pixels)
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var size = (await source.GetInfoAsync()).ImageSize;
                var R = AspectRatioHelper.Aspect(Convert.ToInt32(size.Width), Convert.ToInt32(size.Height));
                Frame.Navigate(typeof(FinalizeAddView), (FiltersList.SelectedItem as FilterListItem).bitmapSource);
                //var res = await AppCore.InstaApi.UploadPhotoAsync(
                //    new InstaSharper.Classes.Models.InstaImage((FiltersList.SelectedItem as FilterListItem).bitmapSource.LocalPath, (int)size.Width, (int)size.Height), "#تست #موقت");
            }

            StorageFile F2S = null;
            if (FiltersList.SelectedItem == null)
                F2S = imageStorageFile;

            else
                F2S = await StorageFile.GetFileFromApplicationUriAsync((FiltersList.SelectedItem as FilterListItem).bitmapSource);

            var fsp = new FileSavePicker();
            fsp.FileTypeChoices.Add(".jpg", new List<string> { ".jpg" });
            fsp.SuggestedFileName = "WinGoTag";
            fsp.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            var fs = await fsp.PickSaveFileAsync();
            if (fs == null)
                return;
            await F2S.CopyAndReplaceAsync(fs);
            //    using (var source = new StorageFileImageSource(imageStorageFile))
            //    using (var contrastEffect = new BlurEffect(source) { KernelSize = 40 })
            //    using (var renderer = new JpegRenderer(contrastEffect, JpegOutputColorMode.Yuv420))
            //    {
            //        var info = await source.GetInfoAsync();
            //        var saveAsTarget = await ApplicationData.Current.LocalFolder.CreateFileAsync("TempImage1.Jpg", CreationCollisionOption.OpenIfExists);
            //        var render = await renderer.RenderAsync();
            //        using (var fs = await saveAsTarget.OpenAsync(FileAccessMode.ReadWrite))
            //        {
            //            await fs.WriteAsync(render);
            //            await fs.FlushAsync();
            //        }
            //    }
            //    var res = await AppCore.InstaApi.UploadPhotoAsync(new InstaSharper.Classes.Models.InstaImage()
            //    {
            //        URI = new Uri("ms-appdata:///TempImage1.Jpg", UriKind.Absolute).LocalPath,
            //        Width = 391,
            //        Height = 428
            //    }, "از بیرون تحریم؛ از داخل فیلتر :|");
        }

        bool SupportedAspectRatio(string Aspect)
        {
            var sp = Aspect.Split(':');
            if (int.Parse(sp[1]) > int.Parse(sp[0]))
            {
                return false;
            }
            if (int.Parse(sp[0]) < 4 && int.Parse(sp[0]) > 1 && int.Parse(sp[1]) > 1 && int.Parse(sp[1]) < 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}