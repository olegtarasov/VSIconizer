//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace NotLimited.Framework.Common.Helpers
{
	public static class ReflectionHelper
	{
		public static CustomAttributeBuilder ToAttributeBuilder(this CustomAttributeData data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			var constructorArguments = data.ConstructorArguments.Select(x => x.Value).ToList();
			var propertyArguments = new List<PropertyInfo>();
			var propertyArgumentValues = new List<object>();
			var fieldArguments = new List<FieldInfo>();
			var fieldArgumentValues = new List<object>();

			if (data.NamedArguments != null)
			{
				foreach (var namedArg in data.NamedArguments)
				{
					var fi = namedArg.MemberInfo as FieldInfo;
					var pi = namedArg.MemberInfo as PropertyInfo;

					if (fi != null)
					{
						fieldArguments.Add(fi);
						fieldArgumentValues.Add(namedArg.TypedValue.Value);
					}
					else if (pi != null)
					{
						propertyArguments.Add(pi);
						propertyArgumentValues.Add(namedArg.TypedValue.Value);
					}
				}
			}

			return new CustomAttributeBuilder(
				data.Constructor,
				constructorArguments.ToArray(),
				propertyArguments.ToArray(),
				propertyArgumentValues.ToArray(),
				fieldArguments.ToArray(),
				fieldArgumentValues.ToArray());
		}

	}
}