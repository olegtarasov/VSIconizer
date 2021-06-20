using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;

using VSIconizer.Core;

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

        private VSIconizerService iconizerService;

        protected override void Initialize()
        {
            base.Initialize();

            if (!ReflectedVSControlMethods.TryCreate( out ReflectedVSControlMethods vsMethods))
            {
                throw new InvalidOperationException("Failed to safely reflect required VS shell types.");
            }

            IconizerOptionPage optionsPage = (IconizerOptionPage)this.GetDialogPage(typeof(IconizerOptionPage));

            VSIconizerConfiguration cfg = optionsPage.ToVSIconizerConfiguration();

            optionsPage.OptionsUpdated += this.OnOptionsUpdated;

            this.iconizerService = new VSIconizerService(
//              dte       : (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE)),
//              events    : ,
                vsMethods : vsMethods,
                currentCfg: cfg
            );
        }

        private void OnOptionsUpdated(object sender, OptionsUpdatedEventArgs e)
        {
            if (this.iconizerService != null)
            {
                this.iconizerService.ApplyConfiguration(e.Configuration);
            }
        }

        protected override int QueryClose(out bool canClose)
        {
            this.iconizerService.Shutdown();
            return base.QueryClose(out canClose);
        }
    }
}
