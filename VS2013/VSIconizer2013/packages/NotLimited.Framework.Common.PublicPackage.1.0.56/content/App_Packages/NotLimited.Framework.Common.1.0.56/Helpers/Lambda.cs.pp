//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;

namespace NotLimited.Framework.Common.Helpers
{
	public static class Lambda<T>
	{
		public static Expression<Func<T, TKey>> Expr<TKey>(Expression<Func<T, TKey>> expression)
		{
			return expression;
		}

		public static string MemberName<TKey>(Expression<Func<T, TKey>> expression)
		{
			return expression.GetMemberName();
		}

	    public static LambdaBuilder<T> MemberList()
	    {
	        return new LambdaBuilder<T>();
	    }
	}

    public class LambdaBuilder<T>
    {
        private readonly List<LambdaExpression> _expressions = new List<LambdaExpression>();

        public LambdaBuilder<T> Expr<TKey>(Expression<Func<T, TKey>> expression)
        {
            _expressions.Add(expression);
            return this;
        }

        public IEnumerable<string> AsEnumerable()
        {
            return (HashSet<string>)this;
        }

        public string[] ToArray()
        {
            return _expressions.Select(x => x.GetMemberName()).ToArray();
        }

        public static implicit operator HashSet<string>(LambdaBuilder<T> builder)
        {
            return builder._expressions.Select(x => x.GetMemberName()).ToHashSet();
        }
    }
}