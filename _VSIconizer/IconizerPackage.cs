//------------------------------------------------------------------------------
// <copyright file="IconizerPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Runtime.InteropServices;
using Microsoft.VisualStudio.PlatformUI.Shell.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VsIconizer.Core;
using VSIconizer.Resources;

namespace VSIconizer
{
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
	[Guid(GuidList.IconzerPackageGuid)]
	[ProvideAutoLoad(UIContextGuids80.NoSolution)]
	public sealed class IconizerPackage : Package
	{
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
