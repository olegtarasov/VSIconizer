//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows;

namespace NotLimited.Framework.Wpf
{
	public class ThemeFixer
	{
		// Methods
		public static void FixThemeIfNeeded()
		{
			if (IsCurrentThemeSupported())
				return;

			string theme = null;
			var winVer = System.Environment.OSVersion.Version;
			var clrVer = System.Environment.Version;

			if (winVer.Major == 5 && winVer.Minor >= 1)
				theme = "luna";
			if (winVer.Major == 6 && winVer.Minor < 2)
				theme = "aero";
			if (winVer.Major == 6 && winVer.Minor >= 2)
				theme = "aero2";

			string uri = "PresentationFramework." + theme + ", Version=" + clrVer.Major.ToString() + ".0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/" + theme + ".normalcolor.xaml";

			var dictionary = Application.LoadComponent(new Uri(uri, UriKind.Relative)) as ResourceDictionary;
			if (dictionary != null)
				Application.Current.Resources.MergedDictionaries.Add(dictionary);
		}

		[SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		private static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int dwMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);
		private static string GetThemeFile()
		{
			var pszThemeFileName = new StringBuilder(260);
			GetCurrentThemeName(pszThemeFileName, pszThemeFileName.Capacity, null, 0, null, 0);
			return pszThemeFileName.ToString().ToLower();
		}

		public static bool IsCurrentThemeSupported()
		{
			string themeFile = GetThemeFile();
			return (themeFile.EndsWith("aero.msstyles") || themeFile.EndsWith("luna.msstyles"));
		}
	}
}
