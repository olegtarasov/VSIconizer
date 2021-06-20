//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Globalization;
using System.Windows.Data;

namespace NotLimited.Framework.Wpf.Converters
{
	[ValueConversion(typeof(string), typeof(string))]
	public class AppendTextConverter : IValueConverter
	{
		public object Convert(object obj, Type targetType, object parameter, CultureInfo culture)
		{
			var value = obj as string;
			if (value == null)
				return null;

			if (parameter == null)
				return value;

			return value + parameter.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}