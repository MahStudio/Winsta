using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Microsoft.Graphics.Canvas.Effects;

namespace MGS.Controls
{
    /// <summary>
    /// A lightweight control to add blur and tint effect.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Control" />
    public class BackDrop : Control
    {
        private readonly Compositor _compositor;
        private readonly SpriteVisual _blurVisual;
        private readonly CompositionBrush _blurBrush;

        /// <summary>
        /// The blur amount property.
        /// </summary>
        public static readonly DependencyProperty BlurAmountProperty =
            DependencyProperty.Register(
                nameof(BlurAmount),
                typeof(double),
                typeof(BackDrop),
                new PropertyMetadata(10d, OnBlurAmountChanged));

        /// <summary>
        /// The tint color property.
        /// </summary>
        public static readonly DependencyProperty TintColorProperty =
            DependencyProperty.Register(
                nameof(TintColor),
                typeof(Color),
                typeof(BackDrop),
                new PropertyMetadata(Colors.Transparent, OnTintColorChanged));

        /// <summary>
        /// The tint alpha property.
        /// </summary>
        public static readonly DependencyProperty TintAlphaProperty =
            DependencyProperty.Register(
                nameof(TintAlpha),
                typeof(int),
                typeof(BackDrop),
                new PropertyMetadata(90, OnTintAlphaChanged));

        /// <summary>
        /// The saturation intensity property.
        /// </summary>
        public static readonly DependencyProperty SaturationIntensityProperty =
            DependencyProperty.Register(
                nameof(SaturationIntensity),
                typeof(double),
                typeof(BackDrop),
                new PropertyMetadata(1.75, OnSaturationIntensityChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="BackDrop"/> class.
        /// </summary>
        public BackDrop()
        {
            var rootVisual = ElementCompositionPreview.GetElementVisual(this);
            _compositor = rootVisual.Compositor;
            _blurVisual = _compositor.CreateSpriteVisual();

            var brush = BuildBlurBrush();
            brush.SetSourceParameter("Source", _compositor.CreateBackdropBrush());
            _blurBrush = brush;
            _blurVisual.Brush = _blurBrush;

            TintColor = Colors.Transparent;

            ElementCompositionPreview.SetElementChildVisual(this, _blurVisual);

            Loading += OnLoading;
            Unloaded += OnUnloaded;
        }

        /// <summary>
        /// Gets or sets the blur amount.
        /// </summary>
        /// <value>The blur amount.</value>
        public double BlurAmount
        {
            get { return (double)GetValue(BlurAmountProperty); }
            set { SetValue(BlurAmountProperty, value); }
        }

        /// <summary>
        /// Gets or sets the tint color.
        /// </summary>
        /// <value>The tint color.</value>
        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the tint alpha.
        /// </summary>
        /// <value>The tint alpha.</value>
        public int TintAlpha
        {
            get { return (int)GetValue(TintAlphaProperty); }
            set { SetValue(TintAlphaProperty, value); }
        }

        /// <summary>
        /// Gets or sets the saturation intensity.
        /// </summary>
        /// <value>The saturation intensity.</value>
        public double SaturationIntensity
        {
            get { return (double)GetValue(SaturationIntensityProperty); }
            set { SetValue(SaturationIntensityProperty, value); }
        }

        private static void OnBlurAmountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var backDrop = d as BackDrop;

            if (backDrop == null) return;

            backDrop._blurBrush.Properties.InsertScalar("Blur.BlurAmount", (float)(double)e.NewValue);
        }

        private static void OnTintColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var backDrop = d as BackDrop;

            if (backDrop == null) return;

            var color = (Color)e.NewValue;
            color.A = (byte)backDrop.TintAlpha;

            backDrop._blurBrush.Properties.InsertColor("Color.Color", color);
        }

        private static void OnTintAlphaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var backDrop = d as BackDrop;

            if (backDrop == null) return;

            var color = backDrop.TintColor;
            color.A = (byte)(int)e.NewValue;

            backDrop._blurBrush.Properties.InsertColor("Color.Color", color);
        }

        private static void OnSaturationIntensityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var backDrop = d as BackDrop;

            if (backDrop == null) return;

            backDrop._blurBrush.Properties.InsertScalar("Saturation.Saturation", (float)(double)e.NewValue);
        }

        private void OnLoading(FrameworkElement sender, object args)
        {
            SizeChanged += OnSizeChanged;
            OnSizeChanged(this, null);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_blurVisual != null)
            {
                _blurVisual.Size = new System.Numerics.Vector2((float)ActualWidth, (float)ActualHeight);
            }
        }

        private CompositionEffectBrush BuildBlurBrush()
        {
            var blurEffect = new GaussianBlurEffect()
            {
                Name = "Blur",
                BlurAmount = 0.0f,
                BorderMode = EffectBorderMode.Hard,
                Optimization = EffectOptimization.Balanced,
                Source = new CompositionEffectSourceParameter("Source"),
            };

            var blendEffect = new BlendEffect
            {
                Background = blurEffect,
                Foreground = new ColorSourceEffect()
                {
                    Name = "Color",
                    Color = Color.FromArgb(90, 255, 255, 255)
                },
                Mode = BlendEffectMode.SoftLight
            };

            var saturationEffect = new SaturationEffect
            {
                Name = "Saturation",
                Source = blendEffect,
                Saturation = 1.75f
            };

            var factory = _compositor.CreateEffectFactory(
                saturationEffect,
                new[] { "Blur.BlurAmount", "Color.Color", "Saturation.Saturation" });

            return factory.CreateBrush();
        }
    }
}
