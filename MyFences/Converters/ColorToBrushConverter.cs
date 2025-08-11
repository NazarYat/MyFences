using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace MyFences.Converters
{
    public sealed class ColorToBrushConverter : MarkupExtension, IValueConverter
    {
        // Reuse a single instance (stateless) to reduce object creation in XAML.
        private static ColorToBrushConverter _instance;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ??= new ColorToBrushConverter();
        }

        // Convert Color or string -> SolidColorBrush
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Determine base color
            Color color;

            if (value is Color c)
            {
                color = c;
            }
            else if (value is SolidColorBrush brush) // user sometimes passes brush already
            {
                return FreezeBrush(new SolidColorBrush(brush.Color), parameter);
            }
            else if (value is string s && !string.IsNullOrWhiteSpace(s))
            {
                // Try parsing hex or known color names
                try
                {
                    var converted = (Color)ColorConverter.ConvertFromString(s.Trim());
                    color = converted;
                }
                catch
                {
                    // can't parse -> transparent as fallback
                    color = Colors.Transparent;
                }
            }
            else
            {
                // Not a color-like value: fall back to Transparent
                color = Colors.Transparent;
            }

            var resultBrush = new SolidColorBrush(color);
            return FreezeBrush(resultBrush, parameter);
        }

        // ConvertBack: Brush -> Color (returns Color if possible)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush scb)
            {
                return scb.Color;
            }

            if (value is Brush anyBrush)
            {
                // try to extract via brush's fallback
                var defaultColor = Colors.Transparent;
                return defaultColor;
            }

            return Binding.DoNothing;
        }

        // Helper: parse opacity from parameter (accepts string or double)
        private static double ParseOpacity(object parameter)
        {
            if (parameter == null) return -1;
            if (parameter is double d) return Clamp01(d);
            if (parameter is float f) return Clamp01(f);
            if (parameter is string s)
            {
                s = s.Trim();
                if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
                    return Clamp01(parsed);
            }

            return -1;
        }

        private static double Clamp01(double v) =>
            v < 0 ? 0 : (v > 1 ? 1 : v);

        // Freeze to improve performance (Freezable objects should be frozen when possible)
        private static Brush FreezeBrush(SolidColorBrush brush, object parameter)
        {
            // Keep brush frozen; if caller needs to modify the brush later they should create their own instance.
            if (brush.CanFreeze && !brush.IsFrozen)
            {
                brush.Freeze();
            }

            return brush;
        }
    }
}
