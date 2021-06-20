//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;

namespace NotLimited.Framework.Common.Misc
{
	public struct MinMax<T>
	{
		public T Min, Max;
	}

	public static class AwesomeMath
	{
		public static double Max(params double[] nums)
		{
			double max = double.MinValue;

			for (int i = 0; i < nums.Length; i++)
				if (nums[i] > max)
					max = nums[i];

			return max;
		}

		public static double Min(params double[] nums)
		{
			double min = double.MaxValue;

			for (int i = 0; i < nums.Length; i++)
				if (nums[i] < min)
					min = nums[i];

			return min;
		}

		public static void MinMax(double a, double b, out double min, out double max)
		{
			if (a > b)
			{
				min = b;
				max = a;
			}
			else
			{
				min = a;
				max = b;
			}
		}

		public static MinMax<double> MinMax(params double[] nums)
		{
			var result = new MinMax<double> {Max = double.MinValue, Min = double.MaxValue};

			for (int i = 0; i < nums.Length; i++)
			{
				if (nums[i] > result.Max)
					result.Max = nums[i];
				if (nums[i] < result.Min)
					result.Min = nums[i];
			}

			return result;
		}

		public static bool EqualsEps(this double a, double b, double epsilon = 0.000001f)
		{
			return (Math.Abs(a - b) < epsilon);
		}

		public static int GetDecimalPlaces(float value)
		{
			int result = 0;
			
			while (((int)(value *= 10)) % 10 != 0)
				result++;

			return result;
		}

		public static double RoundedStep(double value, int nums = 1)
		{
			if (value == 0)
				return 0;

			long integral = (long)value;

			if (integral != 0)
				return integral;

			double tmp = value;
			int cnt = 1;
			
			while (((long)(tmp *= 10)) == 0)
				cnt++;

			if (nums > 1)
			{
				cnt += nums - 1;
				tmp *= Math.Pow(10, nums - 1);
			}

			return ((long)tmp) / Math.Pow(10, cnt);
		}
	}
}