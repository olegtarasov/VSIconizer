//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System.Windows;
using System.Windows.Media.Animation;

namespace NotLimited.Framework.Wpf.Animations
{
	public static class AnimationFactory
	{
		public static DoubleAnimation DoubleAnimation(double to, Duration duration, double acceleration = 0.0d)
		{
			return new DoubleAnimation
			{
				To = to,
				AccelerationRatio = acceleration,
				DecelerationRatio = acceleration,
				Duration = duration
			};
		}

		public static DoubleAnimation DoubleAnimation(double from, double to, Duration duration, double acceleration = 0.0d)
		{
			return new DoubleAnimation
			{
				From = from,
				To = to,
				AccelerationRatio = acceleration,
				DecelerationRatio = acceleration,
				Duration = duration
			};
		}
	}
}