//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System.Diagnostics;

namespace NotLimited.Framework.Common.Helpers
{
    public static class EnvironmentHelpers
    {
        public static void ShellOpen(string fileName)
        {
            Process.Start(new ProcessStartInfo(fileName) {UseShellExecute = true});
        }
    }
}