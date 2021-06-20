using System;
using System.Collections.Generic;

using VSIconizer.Core;

namespace VSIconizer
{
#if USE_DATA_GRID_VIEW
    using GdiColor  = System.Drawing.Color;
    using WpfColor  = System.Windows.Media.Color;
    using WpfColors = System.Windows.Media.Colors;

    public partial class IconizerOptionsControl
    {
        private class TabColorEditorRow
        {
            public static TabColorEditorRow FromWpfColor(string tabText, WpfColor wpfColor)
            {
                return new TabColorEditorRow()
                {
                    // Set fields directly, so it doesn't go through the setters:
                    tabTextValue   = tabText,
                    wpfColorValue  = wpfColor,
                    colorTextValue = wpfColor.TryGetColorName(out string name) ? name : wpfColor.ToString()
                };
            }

            //

            private string   tabTextValue;
            private string   colorTextValue;
            private WpfColor wpfColorValue = WpfColors.Transparent;

            public TabColorEditorRow() // BindingList<T> needs a public parameterless ctor...
            {
            }

            public string TabText
            {
                get => this.tabTextValue ?? String.Empty;
                set => this.tabTextValue = value ?? String.Empty;
            }

            public string ColorText
            {
                get => this.colorTextValue;
                set
                {
                    if (TabColorsSerialization.TryParseWpfColor(value, out WpfColor parsed))
                    {
                        this.colorTextValue = value;
                        this.wpfColorValue = parsed;
                    }
                    else
                    {
                        this.colorTextValue = value;
                        this.wpfColorValue = WpfColors.Transparent;
                    }
                }
            }

            /// <summary>The setter will set both <see cref="WpfColor"/> and <see cref="ColorText"/>.</summary>
            public WpfColor WpfColor
            {
                get => this.wpfColorValue;
                set
                {
                    this.wpfColorValue  = value;
                    this.colorTextValue = value.ToColorNameOrString();
                }
            }

            public GdiColor GdiColor => this.WpfColor.ToGdiColor();

            public bool IsValid => this.Validate();

            private bool Validate()
            {
                return
                    !string.IsNullOrWhiteSpace(this.TabText)
                    && 
                    TabColorsSerialization.TryParseWpfColor(this.colorTextValue, out WpfColor parsed)
                    &&
                    parsed == this.wpfColorValue;
            }
        }
    }
#endif
}
