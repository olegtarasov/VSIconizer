﻿using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Window = System.Windows.Window;

namespace VSIconizer
{
    public class VsIconizerService
    {
        private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        private static readonly Type _imageType = typeof(Image);
        private static readonly Type _textBlock = typeof(TextBlock);

        private const uint TimerInterval = 2000;

        private Timer _timer = null;
        private uint _threadId;
        private Dispatcher _dispatcher;

        private readonly Func<DependencyObject, bool> _isAutohideControl;
        private readonly Func<DependencyObject, bool> _isDragUnlockHeader;
        private readonly MethodInfo _getOrientation;

        public Thickness IconMargin { get; set; }
        public bool ShowText { get; set; }

        public VsIconizerService(Func<DependencyObject, bool> isAutohideControl, Func<DependencyObject, bool> isDragUnlockHeader, Thickness margin, bool showText)
        {
            _isAutohideControl = isAutohideControl;
            _isDragUnlockHeader = isDragUnlockHeader;
            IconMargin = margin;
            ShowText = showText;

            var ass = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x?.FullName.StartsWith("Microsoft.VisualStudio.Shell.ViewManager") == true);
            var type = ass.GetType("Microsoft.VisualStudio.PlatformUI.Shell.Controls.AutoHideChannelControl");
            _getOrientation = type.GetMethod("GetOrientation", BindingFlags.Static | BindingFlags.Public);

            _threadId = GetCurrentThreadId();
            _dispatcher = Dispatcher.CurrentDispatcher;
            _timer = new Timer(TimerCallback, null, TimerInterval, Timeout.Infinite);
            ShowText = showText;
        }

        public void Shutdown()
        {
            _timer.Dispose();
            _timer = null;
        }

        private void TimerCallback(object state)
        {
            if (_timer != null)
            {
                EnumThreadWindows(_threadId, EnumCallback, IntPtr.Zero);
            }

            if (_timer != null)
            {
                try
                {
                    _timer.Change(TimerInterval, Timeout.Infinite);
                }
                catch { }
            }
        }

        private bool EnumCallback(IntPtr hWnd, IntPtr lParam)
        {
            var window = HwndSource.FromHwnd(hWnd)?.RootVisual as Window;
            if (window != null && _timer != null)
            {
                try
                {
                    _dispatcher.Invoke(() => ShowIcons(window));
                }
                catch (TaskCanceledException)
                {
                }
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
                if (_isAutohideControl(descendant))
                {
                    ProcessHiddenChannel(descendant);
                    continue;
                }

                if (_isDragUnlockHeader(descendant))
                {
                    foreach (var grid in descendant.GetVisualDescendants().OfType<Grid>().Where(IsTargetGrid))
                    {
                        ProcessGrid(grid, null);
                    }
                }
            }
        }

        private void ProcessHiddenChannel(DependencyObject control)
        {
            Transform transform = null;

            if ((int)_getOrientation.Invoke(null, new[] { control }) == 1)
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
            image.LayoutTransform = transform;
            grid.Background = Brushes.Transparent;

            if (ShowText)
            {
                textBlock.Visibility = Visibility.Visible;
                textBlock.Margin = new Thickness(0, IconMargin.Top, IconMargin.Right, IconMargin.Bottom);
                image.Margin = new Thickness(IconMargin.Left, IconMargin.Top, IconMargin.Right / 2, IconMargin.Bottom);
                grid.ToolTip = null;
            }
            else
            {
                textBlock.Visibility = Visibility.Collapsed;
                image.Margin = IconMargin;
                grid.ToolTip = textBlock.Text;
            }

            if (_timer == null)
            {
                return;
            }

            if (grid.ColumnDefinitions.Count == 0)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                image.SetValue(Grid.ColumnProperty, 0);
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                textBlock.SetValue(Grid.ColumnProperty, 1);
            }
        }

        private bool IsTargetGrid(Grid grid)
        {
            return grid.Children.Count == 2 && _imageType.IsInstanceOfType(grid.Children[0]) && _textBlock.IsInstanceOfType(grid.Children[1]);
        }
    }
}
