using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

//using EnvDTE;

using Window = System.Windows.Window;

namespace VSIconizer.Core
{
    using WpfColor = System.Windows.Media.Color;

    public class VSIconizerService
    {
//      private readonly DTE             dte;
//      private readonly DTEEvents       events;
        private readonly DispatcherTimer timer;
        private readonly uint            threadId;
        private readonly Dispatcher      dispatcher;

        private readonly IVSControlMethods vsMethods;

        private VSIconizerConfiguration cfg;

        // Mutable precomputed margins and values:
        private Thickness  gridMargin;
        private Thickness? iconHorizontal; // When icons are rotated VS/WPF also handles rotating the margin, so no need for separate `Thickness` instances for vertical and horizontal.
        private Visibility iconVisibility;
        private Visibility textVisibility;

        private readonly Dictionary<WpfColor,SolidColorBrush> brushes = new Dictionary<WpfColor,SolidColorBrush>();

        public VSIconizerService(
//          DTE                     dte,
//          DTEEvents               events,
            IVSControlMethods       vsMethods,
            VSIconizerConfiguration currentCfg
        )
        {
            this.vsMethods = vsMethods  ?? throw new ArgumentNullException(nameof(vsMethods));
            this.cfg       = currentCfg ?? throw new ArgumentNullException(nameof(currentCfg));

//          events.OnStartupComplete += () =>
            {
                this.threadId   = NativeMethods.GetCurrentThreadId();
                this.dispatcher = Dispatcher.CurrentDispatcher;
                this.timer      = new DispatcherTimer(DispatcherPriority.DataBind, this.dispatcher);
                
                this.timer.Interval = TimeSpan.FromSeconds(2);
                this.timer.Tick += this.Timer_Tick;

                this.ApplyConfiguration(currentCfg);

                this.timer.Start();
            };

//          events.OnBeginShutdown += () =>
            {

            };
        }

        public void ApplyConfiguration(VSIconizerConfiguration cfg)
        {
            this.cfg = cfg ?? throw new ArgumentNullException(nameof(cfg));

            this.gridMargin = new Thickness(
                left  : cfg.HorizontalSpacing,
                top   : cfg.VerticalSpacing,
                right : cfg.HorizontalSpacing,
                bottom: cfg.VerticalSpacing
            );

            this.iconVisibility = cfg.Mode.ShowIcon() ? Visibility.Visible : Visibility.Collapsed;
            this.textVisibility = cfg.Mode.ShowText() ? Visibility.Visible : Visibility.Collapsed;

            if (cfg.Mode == VSIconizerMode.IconAndText)
            {
                this.iconHorizontal = new Thickness(left: 0, top: 0, right: cfg.IconTextSpacing, bottom: 0);
            }
            else
            {
                this.iconHorizontal = null;
            }

            if (cfg.UseTabColors)
            {
                foreach(WpfColor wpfColor in cfg.TabColors.Values)
                {
                    if(!this.brushes.ContainsKey(wpfColor))
                    {
                        SolidColorBrush scb = new SolidColorBrush(wpfColor);
                        if (scb.CanFreeze) scb.Freeze();

                        this.brushes.Add(wpfColor, scb);
                    }
                }
            }

            //

            // Apply settings immediately, don't wait up-to 2s for the timer:
            this.Timer_Tick(sender: null, e: EventArgs.Empty);
        }

        public void Shutdown()
        {
            this.timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _ = NativeMethods.EnumThreadWindows(dwThreadId: this.threadId, lpfn: this.EnumCallback, lParam: IntPtr.Zero);
        }

        private bool EnumCallback(IntPtr hWnd, IntPtr lParam)
        {
            if( HwndSource.FromHwnd(hWnd) is HwndSource source && source.RootVisual is Window rootWindow)
            {
                try
                {
                    this.dispatcher.Invoke(() => this.EnsureTabAppearance(rootWindow));
                }
                catch (OperationCanceledException)
                {
                }
            }

            return true;
        }

        private void EnsureTabAppearance(Window window)
        {
            foreach (UIElement descendant in window.GetVisualDescendants().OfType<UIElement>())
            {
                if (this.vsMethods.IsAutoHide(descendant))
                {
                    this.ProcessHiddenChannel(descendant);
                }
                else if (this.vsMethods.IsDragUndockHeader(descendant))
                {
                    foreach (Grid grid in descendant.GetVisualDescendants().OfType<Grid>())
                    {
                        if(IsToolWindowTabContentsGrid(grid, icon: out Image icon, textBlock: out TextBlock textBlock))
                        {
                            this.ProcessGrid(grid, transform: null, icon, textBlock);
                        }
                    }
                }
            }
        }

        private void ProcessHiddenChannel(UIElement control)
        {
            Transform transform = null;

            if (this.vsMethods.GetAutoHideChannelOrientation(control) == Orientation.Vertical)
            {
                if(this.cfg.RotateVerticalTabIcons)
                {
                    transform = new RotateTransform(angle: -90);
                }
            }

            foreach (Grid grid in control.GetVisualDescendants().OfType<Grid>())
            {
                if(IsToolWindowTabContentsGrid(grid, icon: out Image icon, textBlock: out TextBlock textBlock))
                {
                    this.ProcessGrid(grid, transform, icon, textBlock);
                }
            }
        }

        private static bool IsToolWindowTabContentsGrid(Grid grid, out Image icon, out TextBlock textBlock)
        {
            if(grid.Children.Count == 2)
            {
                UIElement maybeCrispImage = grid.Children[0];
                UIElement maybeTextBlock  = grid.Children[1];

                if(maybeCrispImage is Image iconCtrl && maybeTextBlock is TextBlock textBlockCtrl)
                {
                    icon      = iconCtrl;
                    textBlock = textBlockCtrl;

                    if(icon.Source != null)
                    {
                        return true;
                    }
                }
            }

            icon = default;
            textBlock = default;
            return false;
        }

        /// <summary></summary>
        /// <param name="grid">This is the WPF <see cref="Grid"/> inside the tab's <see cref="ContentPresenter"/> that has 2 immediate children: <paramref name="icon"/> and <paramref name="textBlock"/>.</param>
        /// <param name="transform">This is <see langword="null"/> for horizontal tabs, and <see cref="_rotate90"/> for </param>
        private void ProcessGrid(Grid grid, Transform transform, Image icon, TextBlock textBlock)
        {
            if (grid.ColumnDefinitions.Count == 0)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                icon.SetValue(Grid.ColumnProperty, 0);
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                textBlock.SetValue(Grid.ColumnProperty, 1);
            }

            // Grid:
            {
                if (this.cfg.UseTabColors && this.cfg.TabColors.TryGetValue(textBlock.Text, out WpfColor tabColor) && this.brushes.TryGetValue(tabColor, out SolidColorBrush brush))
                {
                    if (grid.Background is null)
                    {
                        grid.Background = brush;
                    }
                }
                else
                {
                    grid.Background = Brushes.Transparent; // <-- why is this being done on every invocation?
                }

                grid.Margin  = this.gridMargin;
                grid.ToolTip = (this.textVisibility == Visibility.Visible) ? null : textBlock.Text;
            }

            // Icon and Text:
            {
                icon     .Visibility = this.iconVisibility;
                textBlock.Visibility = this.textVisibility;

                icon.LayoutTransform = transform;

                if(this.iconHorizontal.HasValue)
                {
                    icon.Margin = this.iconHorizontal.Value;
                }
            }
        }
    }
}
