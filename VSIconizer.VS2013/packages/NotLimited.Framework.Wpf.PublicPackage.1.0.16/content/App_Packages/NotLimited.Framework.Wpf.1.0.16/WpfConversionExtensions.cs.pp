//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System.Windows;

namespace NotLimited.Framework.Wpf
{
	public static class WpfConversionExtensions
	{
		 public static System.Drawing.Point ToDrawingPoint(this Point point)
		 {
			 return new System.Drawing.Point((int)point.X, (int)point.Y);
		 }
	}
}