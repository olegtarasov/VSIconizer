//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NotLimited.Framework.Common.Helpers
{
    public class TypeWithAttributes<T> where T : Attribute
    {
        public TypeWithAttributes(Type type, List<T> attributes)
        {
            Type = type;
            Attributes = attributes;
        }

        public Type Type { get; set; }
        public List<T> Attributes { get; set; }
    }

    public static class TypeHelper
	{
        public static IEnumerable<TypeWithAttributes<TAttribute>> GetTypesWithAttribute<TType, TAttribute>(bool sameNamespace = false) where TAttribute : Attribute
        {
	        return GetTypesWithAttribute<TAttribute>(typeof(TType), sameNamespace);
        }

	    public static IEnumerable<TypeWithAttributes<TAttribute>> GetTypesWithAttribute<TAttribute>(Type srcType, bool sameNamespace = false) where TAttribute : Attribute
	    {
		    if (String.IsNullOrEmpty(srcType.Namespace))
				throw new InvalidOperationException();

            var types = (IEnumerable<Type>)srcType.Assembly.GetTypes();
            if (sameNamespace)
                types = types.Where(x => x.Namespace != null && x.Namespace.StartsWith(srcType.Namespace));

            return from type in types
                   let attributes = type.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>().ToList()
                   where attributes.Count > 0
                   select new TypeWithAttributes<TAttribute>(type, attributes);
	    }

		public static string GetTypeName(this Type type)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition().Name.Left('`') + "<" +
				       type.GetGenericArguments().Select(x => x.GetTypeName()).Aggregate((a, b) => a + ", " + b) + ">";
			}
			
			return type.Name;
		}

        public static string GetFullTypeName(this Type type)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition().FullName.Left('`') + "<" +
				       type.GetGenericArguments().Select(x => x.GetFullTypeName()).Aggregate((a, b) => a + ", " + b) + ">";
			}

			return type.FullName;
		}

		public static List<Type> GetTypesByInterfacesBaseDir<T>()
		{
			return GetTypesByInterfacesDir<T>(AppDomain.CurrentDomain.BaseDirectory);
		}

		public static List<Type> GetTypesByInterfacesDir<T>(string dir)
		{
			var	result = new List<Type>();

			if (!Directory.Exists(dir))
				throw new DirectoryNotFoundException("Directory " + dir + " not found.");

			foreach (var file in Directory.GetFiles(dir, "*.dll"))
				result.AddRange(GetTypesByInterface<T>(file));

			return result;
		}

		public static List<Type> GetTypesByInterface<T>(string path)
		{
			Assembly ass;

			if (!File.Exists(path))
				throw new FileNotFoundException("Assembly file " + path + " not found.");

			try
			{
				ass = Assembly.LoadFrom(path);
			}
			catch (Exception)
			{
				return new List<Type>(0);
			}

			return GetTypesByInterface<T>(ass);
		}

        public static List<Type> GetTypesByAttribute<T>(this Assembly ass) where T : Attribute
        {
            var result = new List<Type>();
            Type[] types = GetTypes(ass);

            foreach (var type in types)
            {
                if (type == null)
                    continue;
                if (type.IsAbstract) // Skip abstract types
                    continue;

                if (type.GetCustomAttribute<T>() != null)
                    result.Add(type);
            }

            return result;
        }

		public static List<Type> GetTypesByInterface<T>(this Assembly ass)
		{
			var	result = new List<Type>();
			string name = typeof(T).FullName;

			if (name == null)
				throw new InvalidOperationException("Can't get type full name!");

			Type[] types = GetTypes(ass);
			
			
			foreach (var type in types)
			{
				if (type == null)
					continue;
				if (type.IsAbstract) // Skip abstract types
					continue;

				var iface = type.GetInterface(name, true);
				if (iface != null)
					result.Add(type);
			}

			return result;
		}

		public static List<Type> GetTypesSubclassOf<T>(this Assembly ass)
		{
			return GetTypesSubclassOf(ass, typeof(T));
		}

        public static List<Type> GetTypesSubclassOf(this Assembly ass, Type superclass)
        {
            var result = new List<Type>();
            Type[] types = GetTypes(ass);


            foreach (var type in types)
            {
                if (type == null)
                    continue;
                if (type.IsAbstract) // Skip abstract types
                    continue;

                if (type.IsSubclassOf(superclass))
                    result.Add(type);
            }

            return result;
        }

		private static Type[] GetTypes(Assembly ass)
		{
			Type[] types;

			try
			{
				types = ass.GetTypes();
			}
			catch (ReflectionTypeLoadException rtle)
			{
				types = rtle.Types;
			}
			return types;
		}

		public static List<T> CreateInstancesByInterfaceDir<T>(string dir)
		{
			return GetTypesByInterfacesDir<T>(dir).Select(type => (T)Activator.CreateInstance(type)).ToList();
		}

		public static List<T> CreateInstancesByInterface<T>(string path)
		{
			return GetTypesByInterface<T>(path).Select(type => (T)Activator.CreateInstance(type)).ToList();
		}

		public static List<T> CreateInstancesByInterface<T>(this Assembly ass, bool skipNoParameterlessConstructor = false)
		{
            IEnumerable<Type> types = GetTypesByInterface<T>(ass);
		    if (skipNoParameterlessConstructor)
		    {
		        types = types.Where(x => x.GetConstructors().Any(c => c.GetParameters().Length == 0));
		    }
            
            return types.Select(type => (T)Activator.CreateInstance(type)).ToList();
		}

        public static string GetDisplayName(this MemberInfo member)
        {
            var displayAttr = member.GetCustomAttribute<DisplayAttribute>();
            if (displayAttr != null && !String.IsNullOrEmpty(displayAttr.Name))
                return displayAttr.Name;

            var descAttr = member.GetCustomAttribute<DescriptionAttribute>();
            if (descAttr != null && !String.IsNullOrEmpty(descAttr.Description))
                return descAttr.Description;

            return member.Name;
        }
	}
}