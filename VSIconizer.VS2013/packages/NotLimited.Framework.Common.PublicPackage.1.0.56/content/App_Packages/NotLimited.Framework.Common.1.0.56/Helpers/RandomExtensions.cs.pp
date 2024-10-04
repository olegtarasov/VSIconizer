//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;

namespace NotLimited.Framework.Common.Helpers
{
	public static class RandomExtensions
	{
		public static double NextDouble(this Random rnd, double high)
		{
			return rnd.NextDouble() * high;
		}

		public static double NextDouble(this Random rnd, double low, double high)
		{
			return rnd.NextDouble() * (high - low) + low;
		}
	}
}