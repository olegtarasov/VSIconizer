//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NotLimited.Framework.Common.Helpers
{
	public static class PathHelpers
	{
		private static readonly char[] _separators = new[] {Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar};
		
		public static string AppPath { get { return AppDomain.CurrentDomain.BaseDirectory; } }

		public static string CombineAppPath(string path)
		{
			return Path.Combine(AppPath, path);
		}

		public static string CombineAppPath(params string[] args)
		{
			return args.Aggregate(AppPath, Path.Combine);
		}

        public static int GetPathHashCode(string path)
        {
            return path.Trim(_separators).ToUpperInvariant().GetHashCode();
        }

		public static IEnumerable<string> FindFiles(string path, string filter = "*.*")
		{
			var queue = new Queue<string>();
			queue.Enqueue(path);

			while (queue.Count > 0)
			{
				string dir = queue.Dequeue();
				foreach (var file in Directory.GetFiles(dir, filter))
					yield return file;

				foreach (var child in Directory.GetDirectories(dir))
					queue.Enqueue(child);
			}
		}

        public static bool PathEquals(string path1, string path2)
        {
            var parts1 = ExplodePath(path1);
            var parts2 = ExplodePath(path2);

            if (parts1.Length != parts2.Length)
                return false;

            for (int i = 0; i < parts1.Length; i++)
            {
                if (!string.Equals(parts1[i], parts2[i], StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

		public static bool IsFileUnderPath(string filePath, string directory)
		{
			if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(directory))
				return false;

			var fileParts = ExplodePath(filePath);
			var dirParts = ExplodePath(directory);

			if (dirParts.Length > fileParts.Length)
				return false;

			for (int i = 0; i < dirParts.Length; i++)
			{
				if (!string.Equals(dirParts[i], fileParts[i], StringComparison.OrdinalIgnoreCase))
					return false;
			}

			return true;
		}

		public static void DeleteDirectory(string path, Action<string> childPreAction = null)
		{
			if (path == null) throw new ArgumentNullException("path");
			if (!Directory.Exists(path)) throw new DirectoryNotFoundException("Directory doesn't exist!");

			foreach (var file in Directory.GetFiles(path))
			{
				if (childPreAction != null)
					childPreAction(file);

				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}

			foreach (var subDir in Directory.GetDirectories(path))
				DeleteDirectory(subDir, childPreAction);

			Directory.Delete(path);
		}

		public static bool HasExtension(string fileName, string extension)
		{
			return !string.IsNullOrEmpty(fileName)
			       && !string.IsNullOrEmpty(extension)
			       && fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
		}

		public static string CombineWithAssemblyDirectory(string path)
		{
			var location = Assembly.GetExecutingAssembly().Location;
			if (string.IsNullOrEmpty(location))
				throw new InvalidOperationException("Can't get current assembly location");

			return Path.Combine(Path.GetDirectoryName(location), path);
		}

		public static string GetAssemblyDirectory()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}

		public static string EnsureSlash(string path, char directorySeparator = '\\')
		{
			if (string.IsNullOrEmpty(path))
				return path;

			if (path[path.Length - 1] == directorySeparator)
				return path;

			if (path[path.Length - 1] == Path.DirectorySeparatorChar || path[path.Length - 1] == Path.AltDirectorySeparatorChar)
				return path.Substring(0, path.Length - 1) + directorySeparator;

			return path + directorySeparator;
		}

		public static string RebasePath(string path, string source, string target, char directorySeparator = '\\')
		{
			return MakeAbsolute(MakeRelative(path, source, directorySeparator), target, directorySeparator);
		}

		public static string MakeRelative(string path, string relativeTo, char directorySeparator = '\\')
		{
			if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(relativeTo))
				return path;

			var pathParts = ExplodePath(path);
			var relativeParts = ExplodePath(relativeTo);
			int cnt;

			for (cnt = 0; cnt < Math.Min(pathParts.Length, relativeParts.Length); cnt++)
				if (!string.Equals(pathParts[cnt], relativeParts[cnt], StringComparison.OrdinalIgnoreCase))
					break;

			if (cnt == 0)
				return path;

			var sb = new StringBuilder();
			for (int i = 0; i < (relativeParts.Length - cnt); i++)
				sb.Append(@"..").Append(directorySeparator);

			for (int i = cnt; i < pathParts.Length; i++)
			{
				sb.Append(pathParts[i]);
				if (i < pathParts.Length - 1)
					sb.Append(directorySeparator);
			}

			return sb.ToString();
		}

		public static string MakeAbsolute(string path, string relativeTo, char directorySeparator = '\\')
		{
			if (string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(relativeTo))
				return relativeTo;

			if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(relativeTo))
				return path;

			if (Path.IsPathRooted(path))
				return path;

			var pathParts = ExplodePath(path);
			var relativeParts = ExplodePath(relativeTo);
			int cnt;

			for (cnt = 0; cnt < pathParts.Length; cnt++)
				if (pathParts[cnt] != "..")
					break;

			var sb = new StringBuilder();

			for (int i = 0; i < relativeParts.Length - cnt; i++)
				sb.Append(relativeParts[i]).Append(directorySeparator);

			for (int i = cnt; i < pathParts.Length; i++)
			{
				sb.Append(pathParts[i]);
				if (i < pathParts.Length - 1)
					sb.Append(directorySeparator);
			}

			return sb.ToString();
		}

	    public static string[] ExplodePath(string path)
		{
			return path.TrimEnd(_separators).Split(_separators, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}