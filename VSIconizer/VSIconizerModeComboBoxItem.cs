using System;
using System.Linq;

using VSIconizer.Core;
using System.Collections.Generic;

namespace VSIconizer
{
    internal class VSIconizerModeComboBoxItem
    {
        public static IReadOnlyList<VSIconizerModeComboBoxItem> Items { get; } = Enum
            .GetValues(typeof(VSIconizerMode))
            .Cast<VSIconizerMode>()
            .Select(value => new VSIconizerModeComboBoxItem(value))
            .ToList();

        private VSIconizerModeComboBoxItem(VSIconizerMode value)
        {
            this.DisplayText = value.GetDisplayName();
            this.Value       = value;
        }

        public string         DisplayText { get; }
        public VSIconizerMode Value       { get; }
    }
}
