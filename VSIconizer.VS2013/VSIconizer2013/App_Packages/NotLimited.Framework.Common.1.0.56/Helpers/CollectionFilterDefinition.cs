//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NotLimited.Framework.Common.Helpers
{
	public class CollectionFilterDefinition<TItem, TParameter> : IComparer
	{
		private readonly TParameter _parameter;

		public CollectionFilterDefinition(TParameter parameter)
		{
			_parameter = parameter;
		}

		public List<Func<TParameter, TItem, bool>> Filters { get; set; }
		public Func<TParameter, TItem, TItem, int> Comparer { get; set; }

		public static implicit operator Predicate<object>(CollectionFilterDefinition<TItem, TParameter> def)
		{
			return def.Filters == null ? (Predicate<object>)null : item => def.Filters.Any(filter => filter(def._parameter, (TItem)item));
		}

		public int Compare(object x, object y)
		{
			if (Comparer == null)
				throw new InvalidOperationException("Comparer is not set");

			return Comparer(_parameter, (TItem)x, (TItem)y);
		}
	}
}