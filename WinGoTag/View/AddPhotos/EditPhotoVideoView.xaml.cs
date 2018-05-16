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
            SqureBlur,
        }
        StorageFile imageStorageFile;
        StorageItemThumbnail thumbnail;
        public EditPhotoVideoView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            imageStorageFile = e.Parameter as StorageFile;
            thumbnail = await imageStorageFile.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.PicturesView, 100);
            await Task.Delay(1000);
            Contrast(100);
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
    }
}
