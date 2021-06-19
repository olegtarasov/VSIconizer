//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using NotLimited.Framework.Common.Logging;

namespace NotLimited.Framework.Common.Helpers
{
	public class SafeExecutionExceptionArgs : EventArgs
	{
		public SafeExecutionExceptionArgs()
		{
		}

		public SafeExecutionExceptionArgs(Exception exception)
		{
			Exception = exception;
		}

		public Exception Exception { get; set; }
	}

	public class SafeExecutionContext
	{
		private readonly Type _type;
		private readonly Logger _log;

	    #region Exception event
		public static event EventHandler<SafeExecutionExceptionArgs> Exception;
		private static void OnException(SafeExecutionContext sender, Exception ex)
		{
			if (Exception != null)
				Exception(sender, new SafeExecutionExceptionArgs(ex));
		}
		#endregion

		private SafeExecutionContext(Type type)
		{
			_type = type;
			_log = Logger.Get(type);
		}

		public static SafeExecutionContext Get(Type type)
		{
			return new SafeExecutionContext(type);
		}

		public static SafeExecutionContext Get<T>()
		{
			return new SafeExecutionContext(typeof(T));
		}

		public T Logged<T>(string message, Func<Logger, T> action, Action<Logger> final = null, T def = default(T))
		{
			if (!string.IsNullOrEmpty(message))
				_log.Info(message);
			return Logged(action, final, def);
		}

		public T Logged<T>(Func<Logger, T> action, Action<Logger> final = null, T def = default(T))
		{
			try
			{
				return action(_log);
			}
#if !DEBUG
			catch (Exception e)
			{
				_log.Error("Unhandled exception", e);
				OnException(this, e);
				return def;
			}
#endif
			finally
			{
				if (final != null)
					final(_log);
			}
		}

		public void Logged(string message, Action<Logger> action, Action<Logger> final = null)
		{
			_log.Info(message);
			Logged(action, final);
		}

		public void Logged(Action<Logger> action, Action<Logger> final = null)
		{
			try
			{
				action(_log);
			}
#if !DEBUG
			catch (Exception e)
			{
				_log.Error("Unhandled exception", e);
				OnException(this, e);
			}
#endif
			finally
			{
				if (final != null)
					final(_log);
			}
		}
	}
}