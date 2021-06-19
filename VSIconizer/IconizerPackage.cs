using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;

using VsIconizer.Core;

namespace VSIconizer
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(IconizerPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideOptionPage(
        pageType: typeof(IconizerOptionPage),
        categoryName: "Environment",
        pageName: "Iconizer",
        categoryResourceID: 0,
        pageNameResourceID: 0,
        supportsAutomation: true,
        keywords: new[] { "tabs", "tab", "window", "windows", "icon", "icons", "tools", "ui", "iconizer", "iconz" }
    )]
    public sealed class IconizerPackage : Package
    {
        public const string PackageGuidString = "376102e5-d394-4f6b-b994-145fa911c278";

        private VsIconizerService _iconizerService;

        protected override void Initialize()
        {
            base.Initialize();

            var options = (IconizerOptionPage)GetDialogPage(typeof(IconizerOptionPage));
            if (options.HorizontalMargin == null)
                options.HorizontalMargin = 10;
            if (options.VerticalMargin == null)
                options.VerticalMargin = 5;
            if (options.ShowText == null)
                options.ShowText = false;

            options.OptionsUpdated += OnOptionsUpdated;

            _iconizerService = new VsIconizerService(
                (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE)),
                obj => obj.GetType().Name == "AutoHideChannelControl",
                obj => obj.GetType().Name == "DragUndockHeader",
                new Thickness(options.HorizontalMargin.Value, options.VerticalMargin.Value, options.HorizontalMargin.Value, options.VerticalMargin.Value),
                options.ShowText.Value);
        }

        private void OnOptionsUpdated(object sender, EventArgs e)
        {
            var options = (IconizerOptionPage)sender;
            if (_iconizerService != null)
            {
                _iconizerService.IconMargin = new Thickness(options.HorizontalMargin.Value, options.VerticalMargin.Value, options.HorizontalMargin.Value, options.VerticalMargin.Value);
                _iconizerService.ShowText = options.ShowText.Value;
            }
        }

        protected override int QueryClose(out bool canClose)
        {
            _iconizerService.Shutdown();
            return base.QueryClose(out canClose);
        }
    }
}
