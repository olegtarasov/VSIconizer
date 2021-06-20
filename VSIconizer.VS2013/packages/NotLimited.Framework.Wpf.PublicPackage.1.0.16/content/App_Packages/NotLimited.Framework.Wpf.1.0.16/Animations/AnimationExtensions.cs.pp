//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace NotLimited.Framework.Wpf.Animations
{
	public static class AnimationExtensions
	{
		public static Duration ToDuration(this TimeSpan span)
		{
			return new Duration(span);
		}

		public static Timeline Accelerated(this Timeline timeline, double ratio)
		{
			timeline.AccelerationRatio = ratio;
			timeline.DecelerationRatio = ratio;

			return timeline;
		}

		public static Timeline Delayed(this Timeline timeline, long delay)
		{
			timeline.BeginTime = TimeSpan.FromMilliseconds(delay);

			return timeline;
		}

		public static Timeline Delayed(this Timeline timeline, TimeSpan delay)
		{
			timeline.BeginTime = delay;

			return timeline;
		}
	}
}