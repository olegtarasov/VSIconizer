//------------------------------------------------------------------------------
// <copyright file="IconizerPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.PlatformUI.Shell.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using VsIconizer.Core;

namespace VSIconizer.Vs15
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(IconizerPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    public sealed class IconizerPackage : Package
    {
        public const string PackageGuidString = "d0469478-ce6b-42de-baeb-cbb9739ed5b3";

        private VsIconizerService _iconizerService;

        protected override void Initialize()
        {
            base.Initialize();

            _iconizerService = new VsIconizerService(
                (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE)),
                obj => obj is AutoHideChannelControl,
                obj => obj is DragUndockHeader);
        }
    }
}
