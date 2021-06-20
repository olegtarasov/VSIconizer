//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System.Windows;
using System.Windows.Data;

namespace NotLimited.Framework.Wpf
{
	public class Inherit : Binding
	{
		public Inherit(string elementName)
		{
			ElementName = elementName;
			Path = new PropertyPath("DataContext");
		}

		public Inherit(string elementName, string path)
		{
			ElementName = elementName;
			Path = new PropertyPath("DataContext." + path);
		}
	}
}