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

namespace NotLimited.Framework.Common.Types
{
	public class TypeResolver
	{
		private readonly HashSet<string> _resolvePaths = new HashSet<string>();
		private readonly Dictionary<string, Assembly> _assemblies = new Dictionary<string, Assembly>();

		public TypeResolver()
		{
			_resolvePaths.Add(Environment.CurrentDirectory.ToLowerInvariant());

			string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (!string.IsNullOrEmpty(location))
				_resolvePaths.Add(location.ToLowerInvariant());

			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		}

		public IEnumerable<Assembly> Assemblies { get { return _assemblies.Values; } }

		public void LoadLocalAssemblies()
		{
			string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (string.IsNullOrEmpty(location))
				return;

			foreach (var file in Directory.GetFiles(location, "*.dll"))
			{
				var ass = LoadAssemblyFile(file);
				if (ass != null)
					_assemblies.Add(file, ass);
			}
		}

		public void LoadAssemblies(IEnumerable<string> paths)
		{
			foreach (var path in paths)
				LoadAssembly(path);
		}

		public void LoadAssembly(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException("path");

			if (Path.IsPathRooted(path))
			{
				string resolvePath = Path.GetDirectoryName(path);
				if (!string.IsNullOrEmpty(resolvePath))
				{
					foreach (var p in CollectResolvePaths(resolvePath))
						_resolvePaths.Add(p);
				}
			}

			_assemblies.Add(path, ResolveAssembly(path));
		}

		public IEnumerable<Type> GetTypes(Func<Type, bool> predicate)
		{
			return _assemblies.Values.SelectMany(assembly => assembly.GetTypes().Where(predicate));
		}

		public IEnumerable<Type> GetTypes(string assemblyFile, Func<Type, bool> predicate)
		{
			if (!_assemblies.ContainsKey(assemblyFile))
				LoadAssembly(assemblyFile);

			return _assemblies[assemblyFile].GetTypes().Where(predicate);
		}

		public Type GetType(string typeName)
		{
			if (string.IsNullOrEmpty(typeName)) throw new ArgumentNullException("type");

			foreach (var assembly in _assemblies.Values)
			{
				var type = assembly.GetType(typeName);
				if (type != null)
					return type;
			}

			return null;
		}

		public Type GetType(string assemblyPath, string type)
		{
			if (string.IsNullOrEmpty(assemblyPath)) throw new ArgumentNullException("assemblyPath");
			if (string.IsNullOrEmpty(type)) throw new ArgumentNullException("type");

			if (!_assemblies.ContainsKey(assemblyPath))
				LoadAssembly(assemblyPath);

			return _assemblies[assemblyPath].GetType(type, false);
		}

		private Assembly ResolveAssembly(string fileName)
		{
			if (Path.IsPathRooted(fileName))
				return LoadAssemblyFile(fileName);

			foreach (var resolvePath in _resolvePaths)
			{
				string path = Path.Combine(resolvePath, fileName);
				var assembly = LoadAssemblyFile(path);
				if (assembly != null)
					return assembly;
			}

			throw new InvalidOperationException("Can't find the assembly: " + fileName);
		}

		private Assembly LoadAssemblyFile(string path)
		{
			try
			{
				return Assembly.LoadFile(path);
			}
			catch
			{
				return null;
			}
		}

		private IEnumerable<string> CollectResolvePaths(string root)
		{
			var queue = new Queue<string>();
			queue.Enqueue(root);

			while (queue.Count > 0)
			{
				string dir = queue.Dequeue();

				foreach (var child in Directory.GetDirectories(dir))
					queue.Enqueue(child);

				yield return dir.ToLowerInvariant();
			}
		}

		private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			var name = new AssemblyName(args.Name);

			foreach (var resolvePath in _resolvePaths)
			{
				string path = Path.Combine(resolvePath, name.Name + ".dll");
				if (!File.Exists(path))
					continue;

				try
				{
					return Assembly.LoadFile(path);
				}
				catch
				{
				}
			}

			return null;
		}
	}
}