using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

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
            Vibrance,
        }
        StorageFile imageStorageFile;
        StorageItemThumbnail thumbnail;
        BitmapImage bitmapImage;
        public EditPhotoVideoView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            imageStorageFile = e.Parameter as StorageFile;
            thumbnail = await imageStorageFile.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.PicturesView, 100);
            //bitmapImage = await ((ImageFileInfo)e.Parameter).GetImageSourceAsync();
            await Task.Delay(1000);

            EditItem.AddEditItem vm = new EditItem.AddEditItem();
            this.DataContext = vm;
            EditsList.ItemsSource = vm.GridViewItems;

            //Vibrance(100);
            NoFilter();
        }

        void AddFilter(ImageFiltersEnum Filter)
        {

        }

        async void NoFilter()
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var renderer = new SwapChainPanelRenderer(source, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
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
            using (var sharpnessEffect = new ColorBoostEffect(contrastEffect) { Gain = (EffectPercentage / 100) })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void Contrast(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new ContrastEffect(contrastEffect) { Level = (EffectPercentage / 100) })
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
            using (var sharpnessEffect = new ExposureEffect(contrastEffect) { Gain = (EffectPercentage / 100), ExposureMode = ExposureMode.Natural })
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

        async void Grayscale(int RedPercentage, int GreenPercentage, int BluePercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new GrayscaleEffect(contrastEffect) { BlueWeight = (BluePercentage / 100), GreenWeight = (GreenPercentage / 100), RedWeight = (RedPercentage / 100) })
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
            using (var sharpnessEffect = new SharpnessEffect(contrastEffect) { Level = (EffectPercentage/100) })
            using (var renderer = new SwapChainPanelRenderer(sharpnessEffect, m_targetSwapChainPanel))
            {
                await renderer.RenderAsync();
            }
        }

        async void Temperature(int EffectPercentage)
        {
            using (var source = new StorageFileImageSource(imageStorageFile))
            using (var contrastEffect = new ContrastEffect(source) { Level = 0.6 })
            using (var sharpnessEffect = new TemperatureAndTintEffect(contrastEffect) { Temperature = (EffectPercentage/100) })
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
            int value = Convert.ToInt32(SliderContrast.Value);
            if(value is 0) { NoFilter(); return; }
            SqureBlur(value);
        }
    }
}
