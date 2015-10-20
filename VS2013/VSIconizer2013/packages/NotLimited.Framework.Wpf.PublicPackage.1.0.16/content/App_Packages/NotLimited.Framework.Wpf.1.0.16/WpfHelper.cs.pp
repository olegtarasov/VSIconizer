//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace NotLimited.Framework.Wpf
{
	public static class WpfHelper
	{
		public static T LoadResource<T>(string path)
		{
			return LoadResource<T>(path, Assembly.GetCallingAssembly());
		}

		public static T LoadResource<T>(string path, Assembly ass)
		{
			T result = default(T);
			StreamResourceInfo sri;

			try
			{
				sri = Application.GetResourceStream(new Uri("pack://application:,,,/" + ass.GetName().Name + ";component/" + path));
			}
			catch (Exception)
			{
				return result;
			}

			if (sri == null)
				return result;

			if (typeof(T) == typeof(Stream))
				return (T)(object)sri.Stream;

			if (sri.ContentType == "application/xaml+xml")
			{
				result = (T)XamlReader.Load(sri.Stream);
			}
			else if (sri.ContentType.IndexOf("image") >= 0)
			{
				BitmapImage bi = new BitmapImage();

				bi.BeginInit();
				bi.StreamSource = sri.Stream;
				bi.EndInit();

				if (typeof(T) == typeof(ImageSource) || typeof(T) == typeof(BitmapSource))
					result = (T)((object)bi);
				else if (typeof(T) == typeof(Image))
				{
					Image img = new Image {Source = bi};

					result = (T)((object)img);
				}
				else
					throw new InvalidOperationException("Unsupported format requested!");
			}
			sri.Stream.Close();
			sri.Stream.Dispose();

			return result;
		}
	}
}