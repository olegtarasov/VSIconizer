//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NotLimited.Framework.Common.Misc
{
	public static class ObjectFactory
	{
		private static readonly Dictionary<Type, object> cache = new Dictionary<Type, object>();

		public static T CreateObject<T>() where T : class
		{
			var type = typeof(T);

			if (cache.ContainsKey(type))
				return ((Func<T>)cache[type])();

			var expr = Expression.New(type);
			var creator = (Expression<Func<T>>)Expression.Lambda(expr);
			var ctor = creator.Compile();

			cache.Add(type, ctor);
			
			return ctor();
		}

		public static TOut CreateObject<TOut, TIn>(TIn arg1) where TOut : class
		{
			var type = typeof(TOut);

			if (cache.ContainsKey(type))
				return ((Func<TIn, TOut>)cache[type])(arg1);

			var ctor = ((Expression<Func<TIn, TOut>>)GetConstructorLambda(type, typeof(TIn))).Compile();

			cache.Add(type, ctor);

			return ctor(arg1);
		}

		public static TOut CreateObject<TOut, TIn1, TIn2>(TIn1 arg1, TIn2 arg2) where TOut : class
		{
			var type = typeof(TOut);

			if (cache.ContainsKey(type))
				return ((Func<TIn1, TIn2, TOut>)cache[type])(arg1, arg2);

			var ctor = ((Expression<Func<TIn1, TIn2, TOut>>)GetConstructorLambda(type, typeof(TIn1), typeof(TIn2))).Compile();

			cache.Add(type, ctor);

			return ctor(arg1, arg2);
		}

		public static TOut CreateObject<TOut, TIn1, TIn2, TIn3>(TIn1 arg1, TIn2 arg2, TIn3 arg3) where TOut : class
		{
			var type = typeof(TOut);

			if (cache.ContainsKey(type))
				return ((Func<TIn1, TIn2, TIn3, TOut>)cache[type])(arg1, arg2, arg3);

			var ctor = ((Expression<Func<TIn1, TIn2, TIn3, TOut>>)GetConstructorLambda(type, typeof(TIn1), typeof(TIn2), typeof(TIn3))).Compile();

			cache.Add(type, ctor);

			return ctor(arg1, arg2, arg3);
		}

		private static LambdaExpression GetConstructorLambda(Type type, params Type[] paramTypes)
		{
			var parameters = new ParameterExpression[paramTypes.Length];

			for (int i = 0; i < paramTypes.Length; i++)
				parameters[i] = Expression.Parameter(paramTypes[i]);

			return Expression.Lambda(Expression.New(type.GetConstructor(paramTypes), parameters), parameters);
		}
	}
}