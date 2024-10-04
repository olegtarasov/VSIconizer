//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Diagnostics;

namespace NotLimited.Framework.Common.Helpers
{
	public static class PerformanceTimer
	{
		public static TimeSpan MeasureTime(Action action)
		{
			var watch = new Stopwatch();
			watch.Start();
			action();
			watch.Stop();

			return watch.Elapsed;
		}
	}
}