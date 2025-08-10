using System.Windows.Media;
using System.Globalization;

namespace MyFences.Util
{
    public static class ColorHelper
    {
        public static Color FromString(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex) || !hex.StartsWith("#"))
                throw new FormatException("Invalid color format.");

            hex = hex.TrimStart('#');

            byte a = 255, r, g, b;

            if (hex.Length == 8) // RRGGBBAA
            {
                a = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                r = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            }
            else if (hex.Length == 6) // RRGGBB
            {
                r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            }
            else
            {
                throw new FormatException("Color must be in #RRGGBB or #RRGGBBAA format.");
            }

            return Color.FromArgb(a, r, g, b);
        }
    }
}
