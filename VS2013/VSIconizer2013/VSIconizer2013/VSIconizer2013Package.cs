//------------------------------------------------------------------------------
// <copyright file="IconizerPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NotLimited.Framework.Wpf;

namespace NotLimited.VSIconizer2013
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.IconzerPackageGuid)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    public sealed class IconizerPackage : Package
    {
        private static readonly Type _imageType = typeof(Image);
        private static readonly Type _textBlock = typeof(TextBlock);

        private EnvDTE.DTE _dte;
        private EnvDTE.DTEEvents _events;
        private Window _window;
        private Timer _timer = null;

        protected override void Initialize()
        {
            base.Initialize();

            _dte = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
            _events = _dte.Events.DTEEvents;

            _events.OnStartupComplete += () =>
            {
                _window = (Window)HwndSource.FromHwnd(new IntPtr(_dte.MainWindow.HWnd)).RootVisual;
                _timer = new Timer(state => _window.Dispatcher.Invoke(ShowIcons), null, 2000, 2000);
            };

            _events.OnBeginShutdown += () =>
            {
                _timer.Dispose();
            };
        }

        private void ShowIcons()
        {
            var grids = (from descendant in _window.GetVisualDescendants()
                         let name = descendant.GetType().Name
                         where name == "AutoHideTabItem" || name == "DragUndockHeader"
                         let grid = descendant.GetVisualDescendants().OfType<Grid>().FirstOrDefault()
                         where grid != null && IsTargetGrid(grid)
                         select grid)
                .ToList();


            foreach (var grid in grids)
            {
                var image = (Image)grid.Children[0];
                if (image.Source == null)
                {
                    continue;
                }

                image.Visibility = Visibility.Visible;
                image.Margin = new Thickness(10, 5, 10, 5);

                grid.Children[1].Visibility = Visibility.Collapsed;

                if (grid.ColumnDefinitions.Count == 0)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                }
            }
        }

        private bool IsTargetGrid(Grid grid)
        {
            return grid.Children.Count == 2 && _imageType.IsInstanceOfType(grid.Children[0]) && _textBlock.IsInstanceOfType(grid.Children[1]);
        }
    }
}
