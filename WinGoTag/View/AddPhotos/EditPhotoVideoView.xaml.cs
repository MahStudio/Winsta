using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using Lumia.Imaging.Artistic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WinGoTag.View.AddPhotos.EditItem;
using WinGoTag.View.AddPhotos.FilterItem;
using static WinGoTag.View.AddPhotos.PhotoGalleryView;

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
            BrightnessEffect,
            ColorAdjust,
            ColorBoost,
            Contrast,
            Despeckle,
            Exposure,
            GaussianNoise,
            GrayscaleEffect,
            LocalBoost,
            Noise,
            Sharpness,
            Temperature,
            SqureBlur,
            SpotlightEffect,
            Vibrance,
        }
        StorageFile imageStorageFile;
        StorageItemThumbnail thumbnail;
        public static BitmapImage bitmapImage;

        int EffectIndex = -1;
        public EditPhotoVideoView()
        {
            this.InitializeComponent();
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
            AppCore.ModerateBack(Frame.GoBack);

            var Data = ((ImageTo)e.Parameter);

            imageStorageFile = Data.FileImage;
            thumbnail = await imageStorageFile.GetThumbnailAsync(ThumbnailMode.PicturesView, 100);

            bitmapImage = await Data.FullImage.GetImageSourceAsync();

            //await Task.Delay(1000);

            AddFilterItem a = new AddFilterItem();
            this.DataContext = a;
            FiltersList.ItemsSource = a.ListFilterItems;


            //EditItem.AddEditItem vm = new EditItem.AddEditItem();
            //this.DataContext = vm;
            //EditsList.ItemsSource = vm.GridViewItems;
            
            //Vibrance(100);
            NoFilter();
        }

        async void NoFilter()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var renderer = new SwapChainPanelRenderer(source, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void Antique()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new AntiqueEffect(source))
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Cartoon()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new CartoonEffect(source) { DistinctEdges = false })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Emboss()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new EmbossEffect(source) { Level = 0.5 })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Fog()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new FogEffect(source))
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void GrayscaleNegative()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new GrayscaleNegativeEffect(source) { })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void LomoRed()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new LomoEffect(source) { LomoStyle = LomoStyle.Red })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void LomoBlue()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new LomoEffect(source) { LomoStyle = LomoStyle.Blue })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void LomoGreen()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new Lumia.Imaging.Artistic.LomoEffect(source) { LomoStyle = LomoStyle.Green })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void LomoYellow()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new LomoEffect(source) { LomoStyle = LomoStyle.Yellow })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void MagicPen()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new Lumia.Imaging.Artistic.MagicPenEffect(source) {})
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Milky()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new MilkyEffect(source) { })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Mirrror()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new Lumia.Imaging.Artistic.MirrorEffect(source) { })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Moonlight()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new MoonlightEffect(source) { })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Negative()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new Lumia.Imaging.Artistic.NegativeEffect(source) { })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Oil()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new OilyEffect(source) { OilBrushSize = OilBrushSize.Medium })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Paint()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new PaintEffect(source) { Level = 4 })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Posterize()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new PosterizeEffect(source) { ColorComponentValueCount = 10 })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Sepia()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new Lumia.Imaging.Artistic.SepiaEffect(source) { })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void ColorSketch()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new SketchEffect(source) { SketchMode = SketchMode.Color })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void GraySketch()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new SketchEffect(source) { SketchMode = SketchMode.Gray })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Solarize()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new SolarizeEffect(source) { })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void BigNose()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new WarpingEffect(source) { WarpMode = WarpMode.BigNose })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void SpotlightEffect()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            {
                var inf = await source.GetInfoAsync();
                using (var sharpnessEffect = new SpotlightEffect(source) { Position = new Point((inf.ImageSize.Width / 2), (inf.ImageSize.Height / 2)), Radius = (int)((inf.ImageSize.Width / 2) - 100), TransitionSize = 0.8 })
                using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
                {
                    await renderer.RenderAsync();
                }
            }
        }

        async void Brightness(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new BrightnessEffect(contrastEffect) { Level = (EffectPercentage / 100) })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void ColorAdjust(int RedPercentage, int GreenPercentage, int BluePercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new ColorAdjustEffect(contrastEffect) { Blue = (BluePercentage / 100), Green = (GreenPercentage / 100), Red = (RedPercentage / 100) })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void ColorBoost(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new ColorBoostEffect(contrastEffect) { Gain = (EffectPercentage / 50) })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void Contrast(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var sharpnessEffect = new ContrastEffect(source) { Level = (EffectPercentage / 100) })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void Despeckle(int EffectPercentage)
        {
            DespeckleLevel Level = DespeckleLevel.Minimum;
            if (EffectPercentage < 25) Level = DespeckleLevel.Minimum;
            if (EffectPercentage > 25 && EffectPercentage <= 50) Level = DespeckleLevel.Low;
            if (EffectPercentage > 50 && EffectPercentage <= 75) Level = DespeckleLevel.High;
            if (EffectPercentage > 75) Level = DespeckleLevel.Maximum;
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new DespeckleEffect(contrastEffect) { DespeckleLevel = Level })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void Exposure(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new ExposureEffect(contrastEffect) { Gain = (EffectPercentage / 67), ExposureMode = ExposureMode.Natural })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void GaussianNoise(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new GaussianNoiseEffect(contrastEffect) { Level = (EffectPercentage / 100) })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void Grayscale()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var sharpnessEffect = new GrayscaleEffect(source))
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void LocalBoost(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new LocalBoostAutomaticEffect(contrastEffect) { Level = (EffectPercentage / 100) })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void Noise(int EffectPercentage)
        {
            NoiseLevel level = NoiseLevel.Minimum;
            if (EffectPercentage <= 35) level = NoiseLevel.Minimum;
            else if (EffectPercentage > 70) level = NoiseLevel.Maximum;
            else level = NoiseLevel.Medium;
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new NoiseEffect(contrastEffect) { Level = level })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void Sharpness(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new SharpnessEffect(contrastEffect) { Level = (EffectPercentage / 100) })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void Temperature(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new TemperatureAndTintEffect(contrastEffect) { Temperature = (EffectPercentage / 100) })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void Vibrance(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new VibranceEffect(contrastEffect) { Level = (EffectPercentage / 100) })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void SqureBlur(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new BlurEffect(contrastEffect) { KernelSize = EffectPercentage })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }


        private void FiltersList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void ToBackBT_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void SliderContrast_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (EffectIndex is -1) { return; }

            int value = Convert.ToInt32(SliderContrast.Value);
            if (value is 0) { NoFilter(); return; }

            switch (EffectIndex)
            {
                case 0: /*add*/ break;
                case 1: Brightness(value); break;
                case 2: Contrast(value); break;
                case 3: Temperature(value); break;
                case 4: ColorBoost(value); break;
                case 5: /*Add*/ break;
                case 6: /*add*/ break;
            }
        }

        private void EditsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var data = e.ClickedItem as GridViewEditItem;
            EffectIndex = data.Target;
            _NameEffect.Text = data.Text;
            EditRoot.Visibility = Visibility.Visible;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            EditRoot.Visibility = Visibility.Collapsed;
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            EditRoot.Visibility = Visibility.Collapsed;
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            var res = await AppCore.InstaApi.UploadPhotoAsync(new InstaSharper.Classes.Models.InstaImage()
            {
                URI = new Uri("ms-appx:///Logos/perfectColor.png", UriKind.Absolute).LocalPath,
                Width = 391,
                Height = 428
            }, "Test Winsta App Upload Photo");
        }
    }
}
