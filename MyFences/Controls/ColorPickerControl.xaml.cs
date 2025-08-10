using MyFences.Util;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MyFences.Controls
{
    /// <summary>
    /// Interaction logic for ColorPickerControl.xaml
    /// </summary>
    public partial class ColorPickerControl : UserControl, INotifyPropertyChanged
    {
        private double _hue = 0;
        private double _saturation = 1;
        private double _value = 1;
        private double _alpha = 1;

        public double Hue
        {
            get => _hue;
            set
            {
                if (_hue == value) return;

                _hue = value;
                HueSlider.Value = _hue;

                OnPropertyChanged();
                NotifyColorPropertiesChanged();
            }
        }
        public double Saturation
        {
            get => _saturation;
            set
            {
                if (_saturation == value) return;

                _saturation = value;

                OnPropertyChanged();
                NotifyColorPropertiesChanged();
            }
        }
        public double Value
        {
            get => _value;
            set
            {
                if (_value == value) return;

                _value = value;

                OnPropertyChanged();
                NotifyColorPropertiesChanged();
            }
        }
        public double Alpha
        {
            get => _alpha;
            set
            {
                if (_alpha == value) return;

                _alpha = value;
                AlphaSlider.Value = _alpha;

                OnPropertyChanged();
                NotifyColorPropertiesChanged();
            }
        }
        private void NotifyColorPropertiesChanged()
        {
            OnPropertyChanged(nameof(SelectedColorInternal));
            OnPropertyChanged(nameof(SelectedColorHex));
            OnPropertyChanged(nameof(SelectedColorWithAlpha0));
            OnPropertyChanged(nameof(SelectedColorWithAlpha1));
            OnPropertyChanged(nameof(SelectedBrush));
            OnPropertyChanged(nameof(SelectedHueColor));
            MoveThumb(new Point(_saturation * ColorCanvas.Width, (1 - _value) * ColorCanvas.Height));
            SetValue(SelectedColorProperty, SelectedColorInternal);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ColorPickerControl()
        {
            InitializeComponent();
            
        }

        // --- Public bindable properties ---

        public SolidColorBrush SelectedBrush => new SolidColorBrush(SelectedColorInternal);

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(Color),
                typeof(ColorPickerControl),
                new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));

        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set 
            {
                SetValue(SelectedColorProperty, value);

                SetupValuesFromSelectedColor();
            }
        }
        private void SetupValuesFromSelectedColor()
        {
            var color = SelectedColor;

            Alpha = color.A / 255.0;

            var hsv = RGBtoHSV(color.R / 255.0, color.G / 255.0, color.B / 255.0);

            Hue = hsv.h;
            Saturation = hsv.s;
            Value = hsv.v;
        }
        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { }
        public Color SelectedColorInternal => HSVAToColor(Hue, Saturation, Value, Alpha);
        public Color SelectedHueColor => HSVAToColor(Hue, 1, 1, 1);
        public Color SelectedColorWithAlpha0 => HSVAToColor(Hue, Saturation, Value, 0);
        public Color SelectedColorWithAlpha1 => HSVAToColor(Hue, Saturation, Value, 1);
        public string SelectedColorHex
        {
            get => $"#{SelectedColorInternal.A:X2}{SelectedColorInternal.R:X2}{SelectedColorInternal.G:X2}{SelectedColorInternal.B:X2}";
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                try
                {
                    if (SelectedColorHex == value) return;

                    var color = ColorHelper.FromString(value);

                    Alpha = color.A / 255.0;
                    var (h, s, v) = RGBtoHSV(color.R / 255.0, color.G / 255.0, color.B / 255.0);
                    Hue = h;
                    Saturation = s;
                    Value = v;
                }
                catch
                {
                }
            }
        }

        // --- Event handlers ---

        private void HueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Hue = e.NewValue;
        }

        private void AlphaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Alpha = e.NewValue;
        }

        private void ColorCanvas_Mouse(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(ColorCanvas);
                UpdateSaturationAndValue(p.X, p.Y);
            }
        }

        private void ColorCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(ColorCanvas);
                UpdateSaturationAndValue(p.X, p.Y);
                
                if (_isDraggingThumb)
                {
                    MoveThumb(e.GetPosition(ColorCanvas));
                }
            }
        }

        private void ColorDisplay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ColorPopup.IsOpen = true;
            e.Handled = true;
        }

        private void UpdateSaturationAndValue(double x, double y)
        {
            if (_saturation == Math.Clamp(x / ColorCanvas.ActualWidth, 0, 1) && _value == 1.0 - Math.Clamp(y / ColorCanvas.ActualHeight, 0, 1)) return;

            Saturation = Math.Clamp(x / ColorCanvas.ActualWidth, 0, 1);
            Value = 1.0 - Math.Clamp(y / ColorCanvas.ActualHeight, 0, 1);
        }

        private static (double h, double s, double v) RGBtoHSV(double r, double g, double b)
        {
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;

            double h = 0;
            if (delta > 0)
            {
                if (max == r)
                    h = 60 * (((g - b) / delta) % 6);
                else if (max == g)
                    h = 60 * (((b - r) / delta) + 2);
                else
                    h = 60 * (((r - g) / delta) + 4);
            }

            if (h < 0) h += 360;
            double s = (max == 0) ? 0 : delta / max;
            double v = max;

            return (h, s, v);
        }
        private static Color HSVAToColor(double h, double s, double v, double a)
        {
            var c = HSVtoRGB(h, s, v);

            return Color.FromArgb((byte)(a * 255), (byte)(c.r * 255), (byte)(c.g * 255), (byte)(c.b * 255));
        }
        private static (double r, double g, double b) HSVtoRGB(double h, double s, double v)
        {
            h = h % 360;
            double c = v * s;
            double x = c * (1 - Math.Abs((h / 60) % 2 - 1));
            double m = v - c;

            double r1 = 0, g1 = 0, b1 = 0;

            if (h < 60) { r1 = c; g1 = x; b1 = 0; }
            else if (h < 120) { r1 = x; g1 = c; b1 = 0; }
            else if (h < 180) { r1 = 0; g1 = c; b1 = x; }
            else if (h < 240) { r1 = 0; g1 = x; b1 = c; }
            else if (h < 300) { r1 = x; g1 = 0; b1 = c; }
            else { r1 = c; g1 = 0; b1 = x; }

            return (r1 + m, g1 + m, b1 + m);
        }
        private void ColorPopup_Opened(object sender, EventArgs e)
        {
            NotifyColorPropertiesChanged();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private bool _isDraggingThumb;

        private void ColorCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isDraggingThumb = true;
            MoveThumb(e.GetPosition(ColorCanvas));
            ColorCanvas.CaptureMouse();
        }

        private void ColorCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDraggingThumb = false;
            ColorCanvas.ReleaseMouseCapture();
        }

        private void MoveThumb(Point position)
        {
            double x = Math.Clamp(position.X, 0, ColorCanvas.Width);
            double y = Math.Clamp(position.Y, 0, ColorCanvas.Height);

            Canvas.SetLeft(SelectionThumb, x - SelectionThumb.Width / 2);
            Canvas.SetTop(SelectionThumb, y - SelectionThumb.Height / 2);

            // Convert X/Y to saturation/brightness values here
            double saturation = x / ColorCanvas.Width;
            double brightness = 1 - (y / ColorCanvas.Height);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetupValuesFromSelectedColor();
        }
    }
}
