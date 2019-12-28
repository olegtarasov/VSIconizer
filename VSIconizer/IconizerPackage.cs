using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using VsIconizer.Core;

namespace VSIconizer
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(IconizerPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideOptionPage(typeof(IconizerOptionPage), "Environment", "Iconizer", 0, 0, true)]
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

            options.OptionsUpdated += OnOptionsUpdated;

            _iconizerService = new VsIconizerService(
                (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE)),
                obj => obj.GetType().Name == "AutoHideChannelControl",
                obj => obj.GetType().Name == "DragUndockHeader",
                new Thickness(options.HorizontalMargin.Value, options.VerticalMargin.Value, options.HorizontalMargin.Value, options.VerticalMargin.Value));
        }

        private void OnOptionsUpdated(object sender, EventArgs e)
        {
            var options = (IconizerOptionPage)sender;
            if (_iconizerService != null)
                _iconizerService.IconMargin = new Thickness(options.HorizontalMargin.Value, options.VerticalMargin.Value, options.HorizontalMargin.Value, options.VerticalMargin.Value);
        }

        protected override int QueryClose(out bool canClose)
        {
            _iconizerService.Shutdown();
            return base.QueryClose(out canClose);
        }
    }
}
