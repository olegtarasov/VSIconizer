//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Linq.Expressions;

namespace NotLimited.Framework.Common.Helpers
{
	public static class ExpressionHelper
	{
		public static string GetTypeName(this LambdaExpression expression)
		{
			if (expression.Body.NodeType == ExpressionType.MemberAccess)
				return GetTypeName((MemberExpression)expression.Body);
			if (expression.Body.NodeType == ExpressionType.Call)
				return GetTypeName((MethodCallExpression)expression.Body);

			throw new InvalidOperationException("Unsupported expression type!");
		}

		private static string GetTypeName(MethodCallExpression expression)
		{
			if (expression.Object == null)
				throw new InvalidOperationException("Call object is null!");

			return expression.Object.Type.Name;
		}

		private static string GetTypeName(MemberExpression expression)
		{
			throw new InvalidOperationException();
		}

		public static string GetMemberName(this LambdaExpression expression)
		{
		    if (expression.Body.NodeType == ExpressionType.Convert)
		    {
		        var operand = ((UnaryExpression)expression.Body).Operand;
		        if (operand.NodeType == ExpressionType.MemberAccess)
		            return GetMemberName((MemberExpression)operand);
		    }
		    if (expression.Body.NodeType == ExpressionType.MemberAccess)
				return GetMemberName((MemberExpression)expression.Body);
			if (expression.Body.NodeType == ExpressionType.Call)
				return GetMemberName((MethodCallExpression)expression.Body);

			throw new InvalidOperationException("Unsupported expression type!");
		}

		private static string GetMemberName(MethodCallExpression expression)
		{
			return expression.Method.Name;
		}

		private static string GetMemberName(MemberExpression expression)
		{
			return expression.Member.Name;
		}
	}
}