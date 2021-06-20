using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace VSIconizer.Core
{
    using WpfColor  = System.Windows.Media.Color;
    using ColorDict = IReadOnlyDictionary<string,System.Windows.Media.Color>;

    /// <summary>Immutable.</summary>
    public class VSIconizerConfiguration : IEquatable<VSIconizerConfiguration>
    {
        public static ColorDict EmptyTabColors => ImmutableEmptyDictionary<string,System.Windows.Media.Color>.Instance;

        public static VSIconizerConfiguration Default { get; } = new VSIconizerConfiguration(
            mode                  : VSIconizerMode.IconOnly,
            horizontalSpacing     : 10,
            verticalSpacing       : 5,
            iconTextSpacing       : 3,
            rotateVerticalTabIcons: true,
            useTabColors          : false,
            tabColors             : EmptyTabColors
        );

        private static readonly Dictionary<string,string> _emptyDict = new Dictionary<string,string>();

        public VSIconizerConfiguration(VSIconizerMode mode, double horizontalSpacing, double verticalSpacing, double iconTextSpacing, bool rotateVerticalTabIcons, bool useTabColors, ColorDict tabColors)
        {
            this.Mode                   = mode;
            this.HorizontalSpacing      = horizontalSpacing;
            this.VerticalSpacing        = verticalSpacing;
            this.IconTextSpacing        = iconTextSpacing;
            this.RotateVerticalTabIcons = rotateVerticalTabIcons;
            this.UseTabColors           = useTabColors;
            this.TabColors              = tabColors ?? EmptyTabColors;
        }

        public VSIconizerMode Mode { get; }

        /// <summary>
        /// Value for the left and right horizontal spacing between outer edges of content and the tool window tab area.<br />
        /// Value is in WPF Device Independent Units such that each integral unit is 1/96th of an inch, which is 1px at 96dpi (aka 100% DPI), 2px at 192dpi (aka 200% DPI).<br />
        /// So a value of <c>10</c> on a 96dpi display device (with <see cref="Mode"/> = <see cref="VSIconizerMode.Icon"/>) will apply <c>10px</c> of spacing between the left-edge of the icon and the tab edge and another <c>10px</c> on the right side.
        /// </summary>
        public double HorizontalSpacing { get; }

        /// <summary>
        /// Value for the left and right vertical spacing between outer edges of content and the tool window tab area.<br />
        /// Value is in WPF Device Independent Units such that each integral unit is 1/96th of an inch, which is 1px at 96dpi (aka 100% DPI), 2px at 192dpi (aka 200% DPI).<br />
        /// So a value of <c>10</c> on a 96dpi display device (with <see cref="Mode"/> = <see cref="VSIconizerMode.Icon"/>) will apply <c>10px</c> of spacing between the top-edge of the icon and the tab edge and another <c>10px</c> on the bottom side.
        /// </summary>
        public double VerticalSpacing { get; }

        /// <summary>
        /// Spacing between the icon and the text of tool window tab. This option only applies when <see cref="Mode"/> is <see cref="VSIconizerMode.IconAndText"/>.<br />
        /// The spacing is applied by applying a right margin to the icon XAML element - or when a tool window is collapsed then it is applied to the bottom margin to the icon.
        /// </summary>
        public double IconTextSpacing { get; }

        public bool RotateVerticalTabIcons { get; }

        public bool UseTabColors { get; }

        /// <summary>Never null. Can be empty.<br />
        /// The key is the text of the tool window tab. I would like to use some internal/invariant identifier but I don't know how to get it.<br />
        /// The value is a string convertible to a <c>System.Windows.Media.Color</c>, e.g. hexadecimal ARGB.<br />
        /// These entries only have an effect when <see cref="UseTabColors"/> is <see langword="true"/>.
        /// </summary>
        public ColorDict TabColors { get; }

        //

        public VSIconizerConfiguration With(
            VSIconizerMode? mode,
            double?         horizontalSpacing,
            double?         verticalSpacing,
            double?         iconTextSpacing,
            bool?           rotateVerticalTabIcons,
            bool?           useTabColors,
            ColorDict       tabColors
        )
        {
            return new VSIconizerConfiguration(
                mode                  : mode                   ?? this.Mode,
                horizontalSpacing     : horizontalSpacing      ?? this.HorizontalSpacing,
                verticalSpacing       : verticalSpacing        ?? this.VerticalSpacing,
                iconTextSpacing       : iconTextSpacing        ?? this.IconTextSpacing,
                rotateVerticalTabIcons: rotateVerticalTabIcons ?? this.RotateVerticalTabIcons,
                useTabColors          : useTabColors           ?? this.UseTabColors,
                tabColors             : tabColors              ?? this.TabColors
            );
        }


        #region IEquatable

        public override bool Equals(object obj)
        {
            return this.Equals(obj as VSIconizerConfiguration);
        }

        public bool Equals(VSIconizerConfiguration other)
        {
            if (other is null) return false;

            return

                this.Mode                   == other.Mode                   &&
                this.HorizontalSpacing      == other.HorizontalSpacing      &&
                this.VerticalSpacing        == other.VerticalSpacing        &&
                this.IconTextSpacing        == other.IconTextSpacing        &&
                this.RotateVerticalTabIcons == other.RotateVerticalTabIcons &&
                this.UseTabColors           == other.UseTabColors           &&
                ColorDictsEqual(this.TabColors, other.TabColors);
        }

        public override int GetHashCode()
        {
            int hashCode = 276918386;
            hashCode = hashCode * -1521134295 + this.Mode.GetHashCode();
            hashCode = hashCode * -1521134295 + this.HorizontalSpacing.GetHashCode();
            hashCode = hashCode * -1521134295 + this.VerticalSpacing.GetHashCode();
            hashCode = hashCode * -1521134295 + this.IconTextSpacing.GetHashCode();
            hashCode = hashCode * -1521134295 + this.RotateVerticalTabIcons.GetHashCode();
            hashCode = hashCode * -1521134295 + this.UseTabColors.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ColorDict>.Default.GetHashCode(this.TabColors);
            return hashCode;
        }

        public static bool operator ==(VSIconizerConfiguration left, VSIconizerConfiguration right)
        {
            return EqualityComparer<VSIconizerConfiguration>.Default.Equals(left, right);
        }

        public static bool operator !=(VSIconizerConfiguration left, VSIconizerConfiguration right)
        {
            return !( left == right );
        }

        private static bool ColorDictsEqual(ColorDict x, ColorDict y)
        {
            if (x.Count != y.Count) return false;

            // This double-checking is probably redundant...

            foreach (string tabName in x.Keys)
            {
                WpfColor xColor = x[tabName];
                if (y.TryGetValue(tabName, out WpfColor yColor))
                {
                    if (!xColor.Equals(yColor)) return false;
                }
                else
                {
                    return false;
                }
            }

            foreach(string tabName in y.Keys)
            {
                WpfColor yColor = y[tabName];
                if (x.TryGetValue(tabName, out WpfColor xColor))
                {
                    if (!yColor.Equals(xColor)) return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }

    public enum VSIconizerMode
    {
        /// <summary>Use whatever VS uses by default.</summary>
        Default = 0, // <-- This is intentionally the zero-valued (`default`) member.

        /// <summary>Text-only. The default in VS since VS 2012.</summary>
        TextOnly,

        /// <summary>Show only icons in tool window tabs.</summary>
        IconOnly,

        /// <summary>Show both icons and text in VS tool window tabs - the default in VS from Visual InterDev 6.0 through to Visual Studio 2010.</summary>
        IconAndText,

        /// <summary>Hardcore mode: No icons or labels.</summary>
        DasKeyboardMode
    }

    public static class VSIconizerExtensions
    {
        public static bool ShowText(this VSIconizerMode value)
        {
            switch (value)
            {
                case VSIconizerMode.IconOnly:
                case VSIconizerMode.DasKeyboardMode:
                    return false;
                case VSIconizerMode.Default: // Note that normally when `VSIconizerMode.Default` execution will never get to call `ShowText()`.
                case VSIconizerMode.TextOnly:
                case VSIconizerMode.IconAndText:
                default:
                    return true;
            }
        }

        public static bool ShowIcon(this VSIconizerMode value)
        {
            switch (value)
            {
                case VSIconizerMode.IconOnly:
                case VSIconizerMode.IconAndText:
                    return true;
                case VSIconizerMode.DasKeyboardMode:
                case VSIconizerMode.Default:
                case VSIconizerMode.TextOnly:
                default:
                    return false;
            }
        }

        public static string GetDisplayName(this VSIconizerMode value)
        {
            switch (value)
            {
                case VSIconizerMode.Default:
                    return "Visual Studio default";
                case VSIconizerMode.TextOnly:
                    return "Text-only";
                case VSIconizerMode.IconOnly:
                    return "Icon-only";
                case VSIconizerMode.IconAndText:
                    return "Icon and text";
                case VSIconizerMode.DasKeyboardMode:
                    return "Das Keyboard-mode";
                default:
                    return "Unknown value: " + value.ToString();
            }
        }
    }
}
