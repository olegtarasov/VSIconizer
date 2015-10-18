//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;

namespace NotLimited.Framework.Common.Helpers
{
	public static class TimeHelpers
	{
		public static DateTime FromUnixTime(this long seconds)
		{
			var dateTime = new DateTime(1970, 1, 1);
			dateTime = dateTime.AddSeconds((double)seconds);
			return dateTime.ToLocalTime();
		}

		public static long ToUnixTime(this DateTime dateTime)
		{
			return (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
		} 
	}
}