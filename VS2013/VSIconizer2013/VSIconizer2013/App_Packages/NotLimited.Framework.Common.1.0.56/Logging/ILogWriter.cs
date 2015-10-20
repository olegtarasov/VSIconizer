//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
namespace NotLimited.Framework.Common.Logging
{
	/// <summary>
	/// Generic log writer interface
	/// </summary>
	public interface ILogWriter
	{
		/// <summary>
		/// Write a line to the log
		/// </summary>
		/// <param name="text">Text to write</param>
		void WriteLine(string text);

		/// <summary>
		/// Write a message to the log
		/// </summary>
		/// <param name="text">Text to write</param>
		void Write(string text);
	}
}