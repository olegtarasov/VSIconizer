//------------------------------------------------------------------------------
// <copyright file="IconizerPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Microsoft.VisualStudio.Platform.WindowManagement;
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
		private EnvDTE.DTE _dte;
		private EnvDTE.DTEEvents _events;
		private Window _window;

		protected override void Initialize()
		{
			base.Initialize();

			_dte = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
			_events = _dte.Events.DTEEvents;

			_events.OnStartupComplete += () =>
			{
				_window = (Window)HwndSource.FromHwnd(new IntPtr(_dte.MainWindow.HWnd)).RootVisual;
				CommandManager.AddPreviewExecutedHandler(_window, OnPreviewCommandExecuted);
				ShowIcons();
			};
		}

		private void OnPreviewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			var routedCommand = e.Command as RoutedCommand;
			if (routedCommand == null || !string.Equals(routedCommand.Name, "AutoHideView"))
			{
				return;
			}

			var view = e.Parameter as ToolWindowView;

			if (view != null)
			{
				
				view.Hidden += Handler;
				view.Shown += Handler;
			}
		}

		private void Handler(object sender, EventArgs eventArgs)
		{
			ShowIcons();
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
				if (grid.ColumnDefinitions.Count == 0)
				{
					grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
					grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
				}

				for (int i = 0; i < grid.Children.Count; i++)
				{
					var child = (FrameworkElement)grid.Children[i];
					child.SetValue(Grid.ColumnProperty, i);
					child.Visibility = Visibility.Visible;
					child.Margin = new Thickness(2, 0, 2, 0);
				}
			}
		}

		private bool IsTargetGrid(Grid grid)
		{
			return grid.Children.Count == 2 && grid.Children[0].GetType().Name == "CrispImage" && grid.Children[1].GetType().Name == "TextBlock";
		}
	}
}
