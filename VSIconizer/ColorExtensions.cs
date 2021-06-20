using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace VSIconizer
{
    using GdiColor = System.Drawing.Color;
    using WpfColor = System.Windows.Media.Color;

    public static class ColorExtensions
    {
        public static GdiColor ToGdiColor(this WpfColor wpfColor)
        {
            // https://stackoverflow.com/questions/4615779/converting-system-windows-media-color-to-system-drawing-color
            return GdiColor.FromArgb(
                alpha: wpfColor.A,
                red  : wpfColor.R,
                green: wpfColor.G,
                blue : wpfColor.B
            );
        }

        public static WpfColor ToWpfColor(this GdiColor gdiColor)
        {
            return WpfColor.FromArgb(
                a: gdiColor.A,
                r: gdiColor.R,
                g: gdiColor.G,
                b: gdiColor.B
            );
        }

        public static Int32 ToInt32(this WpfColor wpfColor)
        {
            unchecked
            {
                long a = wpfColor.A;
                long r = wpfColor.R;
                long g = wpfColor.G;
                long b = wpfColor.B;

                a = ( a << 24 ) & 0xFF_00_00_00;
                r = ( r << 16 ) & 0x00_FF_00_00;
                g = ( g <<  8 ) & 0x00_00_FF_00;
                b = ( b <<  0 ) & 0x00_00_00_FF;

                long value = a | r | g | b;
                return (int)value;
            }
        }

        private static Lazy<IReadOnlyDictionary<Int32,string>> _wpfColorNameDict = new Lazy<IReadOnlyDictionary<Int32,string>>(valueFactory: GetWpfColorNamesDict);

        private struct WpfColorAndName // ugh, needs ValueTuple
        {
            public static WpfColorAndName CreateFromWpfColorsProperty(PropertyInfo pi)
            {
                Object value = pi.GetValue(obj: null);
                if(value is WpfColor wc)
                {
                    return new WpfColorAndName(pi.Name, color: wc);
                }
                else
                {
                    return new WpfColorAndName(name: null, color: default);
                }
            }

            public WpfColorAndName(string name, WpfColor color)
            {
                this.name    = name;
                this.color   = color;
                this.asInt32 = color.ToInt32();
            }

            public readonly string   name;
            public readonly WpfColor color;
            public readonly Int32    asInt32;
        }

        /// <summary>Builds a dictionary that maps WPF Colors back to their names. The key is the <see cref="Int32"/> representation of a <see cref="WpfColor"/> (see <see cref="ColorExtensions.ToInt32(WpfColor)"/>)</summary>
        private static IReadOnlyDictionary<Int32,string> GetWpfColorNamesDict()
        {
            // https://stackoverflow.com/questions/24468671/extract-color-name-from-a-system-windows-media-color

            return typeof(Colors)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Select(p => WpfColorAndName.CreateFromWpfColorsProperty(p))
                .Where(t => t.name != null)
                .GroupBy(t => t.asInt32) // De-dupe:
                .Select(grp => grp.First())
                .ToDictionary(t => t.asInt32, t => t.name);
        }

        public static bool TryGetColorName(this WpfColor wpfColor, out string name)
        {
            int asInt32 = wpfColor.ToInt32();
            return _wpfColorNameDict.Value.TryGetValue(asInt32, out name);
        }

        public static string ToColorNameOrString(this WpfColor wpfColor)
        {
            return wpfColor.TryGetColorName(out string name) ? name : wpfColor.ToString();
        }
    }
}
