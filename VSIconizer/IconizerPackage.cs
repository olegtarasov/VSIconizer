//------------------------------------------------------------------------------
// <copyright file="IconizerPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.VisualStudio.PlatformUI.Shell.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NotLimited.Framework.Wpf;
using VSIconizer.Resources;

namespace VSIconizer
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
			foreach (var descendant in _window.GetVisualDescendants())
			{
				var channel = descendant as AutoHideChannelControl;
				if (channel != null)
				{
					ProcessHiddenChannel(channel);
					continue;
				}

				var unlockHeader = descendant as DragUndockHeader;
				if (unlockHeader != null)
				{
					foreach (var grid in unlockHeader.GetVisualDescendants().OfType<Grid>().Where(IsTargetGrid))
					{
						ProcessGrid(grid, null);
					}
				}
			}
		}

		private void ProcessHiddenChannel(AutoHideChannelControl control)
		{
			var orientation = AutoHideChannelControl.GetOrientation(control);
			Transform transform = null;

			if (orientation == Orientation.Vertical)
			{
				transform = new RotateTransform(-90);
			}

			foreach (var grid in control.GetVisualDescendants().OfType<Grid>().Where(IsTargetGrid))
			{
				ProcessGrid(grid, transform);
			}
		}

		private void ProcessGrid(Grid grid, Transform transform)
		{
			var image = (Image)grid.Children[0];
			if (image.Source == null)
			{
				return;
			}

			var textBlock = (TextBlock)grid.Children[1];
			image.Visibility = Visibility.Visible;
			image.Margin = new Thickness(10, 5, 10, 5);
			image.LayoutTransform = transform;
			grid.ToolTip = textBlock.Text;

			textBlock.Visibility = Visibility.Collapsed;

			if (grid.ColumnDefinitions.Count == 0)
			{
				grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
				grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			}
		}

		private bool IsTargetGrid(Grid grid)
		{
			return grid.Children.Count == 2 && _imageType.IsInstanceOfType(grid.Children[0]) && _textBlock.IsInstanceOfType(grid.Children[1]);
		}
	}
}
