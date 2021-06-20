//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System.Threading.Tasks;

namespace NotLimited.Framework.Common.Helpers
{
	public static class TaskHelpers
	{
		 public static void MuteExceptions(this Task task)
		 {
			 if (task.Exception != null)
				 task.Exception.Handle(x => true);
		 }
	}
}