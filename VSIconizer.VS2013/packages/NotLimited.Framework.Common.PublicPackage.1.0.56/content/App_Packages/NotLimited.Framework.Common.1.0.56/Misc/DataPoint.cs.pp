//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Runtime.InteropServices;

namespace NotLimited.Framework.Common.Misc
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct DataPoint
	{
		public DataPoint(DateTime time, double price)
		{
			X = time.Ticks;
			Y = price;
		}

		public DataPoint(double x, double y)
		{
			X = x;
			Y = y;
		}

		public double X, Y;

		public DateTime Time
		{
			get { return new DateTime((long)X); } 
			set { X = value.Ticks; }
		}

		public override string ToString()
		{
			return X + "; " + Y;
		}
	}
}