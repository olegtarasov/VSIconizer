using System;
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
using EnvDTE;
using Window = System.Windows.Window;

namespace VsIconizer.Core
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


        private readonly DTE _dte;
        private DTEEvents _events;
        private Timer _timer = null;
        private uint _threadId;
        private Dispatcher _dispatcher;

        private readonly Func<DependencyObject, bool> _isAutohideControl;
        private readonly Func<DependencyObject, bool> _isDragUnlockHeader;
        private readonly MethodInfo _getOrientation;

        public VsIconizerService(DTE dte, Func<DependencyObject, bool> isAutohideControl, Func<DependencyObject, bool> isDragUnlockHeader)
        {
            _isAutohideControl = isAutohideControl;
            _isDragUnlockHeader = isDragUnlockHeader;

            var ass = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x?.FullName.StartsWith("Microsoft.VisualStudio.Shell.ViewManager") == true);
            var type = ass.GetType("Microsoft.VisualStudio.PlatformUI.Shell.Controls.AutoHideChannelControl");
            _getOrientation = type.GetMethod("GetOrientation", BindingFlags.Static | BindingFlags.Public);
            
            _dte = dte;
            _events = _dte.Events.DTEEvents;

            //_events.OnStartupComplete += () =>
            //{
                _threadId = GetCurrentThreadId();
                _dispatcher = Dispatcher.CurrentDispatcher;
                _timer = new Timer(TimerCallback, null, 2000, 2000);
            //};

            //_events.OnBeginShutdown += () =>
            //{
            
            //};
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

            if ((int)_getOrientation.Invoke(null, new []{control}) == 1)
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
