using System;

namespace VSIconizer.Core
{
    /// <summary>Immutable.</summary>
    public class VSIconizerConfiguration
    {
        public static VSIconizerConfiguration Default { get; } = new VSIconizerConfiguration(
            mode                  : VSIconizerMode.IconOnly,
            horizontalSpacing     : 10,
            verticalSpacing       : 5,
            iconTextSpacing       : 3,
            rotateVerticalTabIcons: true
        );

        public VSIconizerConfiguration(VSIconizerMode mode, double horizontalSpacing, double verticalSpacing, double iconTextSpacing, bool rotateVerticalTabIcons)
        {
            this.Mode                   = mode;
            this.HorizontalSpacing      = horizontalSpacing;
            this.VerticalSpacing        = verticalSpacing;
            this.IconTextSpacing        = iconTextSpacing;
            this.RotateVerticalTabIcons = rotateVerticalTabIcons;
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

        //

        public VSIconizerConfiguration With(
            VSIconizerMode? mode,
            double?         horizontalSpacing,
            double?         verticalSpacing,
            double?         iconTextSpacing,
            bool?           rotateVerticalTabIcons
        )
        {
            return new VSIconizerConfiguration(
                mode                  : mode                   ?? this.Mode,
                horizontalSpacing     : horizontalSpacing      ?? this.HorizontalSpacing,
                verticalSpacing       : verticalSpacing        ?? this.VerticalSpacing,
                iconTextSpacing       : iconTextSpacing        ?? this.IconTextSpacing,
                rotateVerticalTabIcons: rotateVerticalTabIcons ?? this.RotateVerticalTabIcons
            );
        }
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
