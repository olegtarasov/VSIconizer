using System.Windows;
using System.Windows.Controls;

namespace VSIconizer.Core
{
    public interface IVSControlMethods
    {
        Orientation GetAutoHideChannelOrientation(UIElement control);

        bool IsAutoHide(UIElement control);

        bool IsDragUndockHeader(UIElement control);
    }
}
