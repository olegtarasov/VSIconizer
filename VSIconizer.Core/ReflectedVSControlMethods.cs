using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using System.Windows;
using System.Windows.Controls;

namespace VSIconizer.Core
{
    using GetOrientationFunc = Func<UIElement,Orientation>;

    /// <summary>Implements <see cref="IVSControlMethods"/> using reflection so we can theoretically support different VS versions without needing hard assembly references.</summary>
    public class ReflectedVSControlMethods : IVSControlMethods
    {
        #region Static factory

        public static bool TryCreate(out ReflectedVSControlMethods instance)
        {
            const string viewManagerAssemblyName = "Microsoft.VisualStudio.Shell.ViewManager";

            IEnumerable<Assembly> currentAssemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(loadedAssembly => loadedAssembly.FullName.StartsWith(viewManagerAssemblyName, StringComparison.OrdinalIgnoreCase));

            foreach (Assembly a in currentAssemblies)
            {
                if (TryCreate(a, out instance))
                {
                    return true;
                }
            }

            instance = default;
            return false;
        }

        public static bool TryCreate(Assembly shellViewManagerAssembly, out ReflectedVSControlMethods instance)
        {
            Type typeofAutoHideChannelControl = shellViewManagerAssembly.GetType("Microsoft.VisualStudio.PlatformUI.Shell.Controls.AutoHideChannelControl");
            Type typeofDragUndockHeader       = shellViewManagerAssembly.GetType("Microsoft.VisualStudio.PlatformUI.Shell.Controls.DragUndockHeader");

            if (typeofAutoHideChannelControl != null && typeofDragUndockHeader != null)
            {
                // public static Orientation GetOrientation(UIElement element)
                MethodInfo getOrientationMethod = typeofAutoHideChannelControl.GetMethod("GetOrientation", BindingFlags.Static | BindingFlags.Public);
                if (getOrientationMethod != null && getOrientationMethod.ReturnType == typeof(System.Windows.Controls.Orientation))
                {
                    GetOrientationFunc getOrientationFunc = (GetOrientationFunc)getOrientationMethod.CreateDelegate(typeof(GetOrientationFunc));

                    instance = new ReflectedVSControlMethods(getOrientationFunc, typeofAutoHideChannelControl, typeofDragUndockHeader);
                    return true;
                }
            }

            instance = default;
            return false;
        }

        #endregion

        private readonly GetOrientationFunc getOrientationFunc;
        private readonly Type               typeofAutoHideChannelControl;
        private readonly Type               typeofDragUndockHeader;

        private ReflectedVSControlMethods(
            GetOrientationFunc getOrientationFunc,
            Type               typeofAutoHideChannelControl,
            Type               typeofDragUndockHeader
        )
        {
            this.getOrientationFunc           = getOrientationFunc           ?? throw new ArgumentNullException(nameof(getOrientationFunc));
            this.typeofAutoHideChannelControl = typeofAutoHideChannelControl ?? throw new ArgumentNullException(nameof(typeofAutoHideChannelControl));
            this.typeofDragUndockHeader       = typeofDragUndockHeader       ?? throw new ArgumentNullException(nameof(typeofDragUndockHeader));
        }

        //

        public Orientation GetAutoHideChannelOrientation(UIElement control)
        {
            return this.getOrientationFunc(control);
        }

        public bool IsAutoHide(UIElement control)
        {
            return this.typeofAutoHideChannelControl.IsInstanceOfType(control);
        }

        public bool IsDragUndockHeader(UIElement control)
        {
            return this.typeofDragUndockHeader.IsInstanceOfType(control);
        }
    }
}
