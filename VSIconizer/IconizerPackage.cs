//------------------------------------------------------------------------------
// <copyright file="IconizerPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
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
		private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

		[DllImport("kernel32.dll")]
		private static extern uint GetCurrentThreadId();

		private static readonly Type _imageType = typeof(Image);
		private static readonly Type _textBlock = typeof(TextBlock);

		private EnvDTE.DTE _dte;
		private EnvDTE.DTEEvents _events;
		private Timer _timer = null;
		private uint _threadId;
		private Dispatcher _dispatcher;

		protected override void Initialize()
		{
			base.Initialize();

			_dte = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
			_events = _dte.Events.DTEEvents;

			_events.OnStartupComplete += () =>
			{
				_threadId = GetCurrentThreadId();
				_dispatcher = Dispatcher.CurrentDispatcher;
				_timer = new Timer(TimerCallback, null, 2000, 2000);
			};

			_events.OnBeginShutdown += () =>
			{
				_timer.Dispose();
				_timer = null;
			};
		}

		private void TimerCallback(object state)
		{
			if (_timer != null)
			{
				EnumThreadWindows(_threadId, EnumCallback, IntPtr.Zero);
			}
		}

		private bool EnumCallback(IntPtr hWnd, IntPtr lParam)
		{
			var window = HwndSource.FromHwnd(hWnd)?.RootVisual as Window;
			if (window != null && _timer != null)
			{
				_dispatcher.Invoke(() => ShowIcons(window));
			}

			return true;
		}

		private void ShowIcons(Window window)
		{
			if (_timer == null)
			{
				return;
			}

			foreach (var descendant in window.GetVisualDescendants())
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

			if (_timer == null)
			{
				return;
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
			grid.Background = Brushes.Transparent;

			textBlock.Visibility = Visibility.Collapsed;

			if (_timer == null)
			{
				return;
			}

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
