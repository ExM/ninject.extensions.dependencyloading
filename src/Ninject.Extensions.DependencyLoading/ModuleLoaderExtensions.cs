using System;
using System.Linq;
using Ninject;
using System.Collections.Generic;
using System.Reflection;

namespace Ninject.Extensions.DependencyLoading
{
	public static class ModuleLoaderExtensions
	{
		public static void LoadModules<T>(this IKernel kernel, IEnumerable<Type> modules,
			Action<T> initialization, Action<List<Type>> warning, Action<List<Type>> error)
		{
			List<Type> sorted = new List<Type>();
			List<Type> remain = new List<Type>();
			
			foreach(Type type in modules)
			{
				T module = (T)kernel.Get(type);
				initialization(module);
				
				sorted.Add(type);
			}
			

		}
		
		
		public static List<T> LoadModules<T, A>(this IKernel kernel, Action<T> initialization, IEnumerable<Type> types)
			where A: Attribute
		{
			List<T> result = new List<T>();
			List<Type> sorted = types.TopologicalSort(TypeDependencyDetector<A>.IsDependency, ShowType);
			
			foreach(Type type in sorted)
			{
				T module = (T)kernel.Get(type);
				result.Add(module);
				initialization(module);
				kernel.ImportServices<A>(module);
			}
			
			return result;
		}
		
		public static List<T> LoadModules<T, A>(this IKernel kernel, Action<T> initialization, params Type[] types)
			where A: Attribute
		{
			return LoadModules<T, A>(kernel, initialization, types.Select((t) => {return t;}));
		}
		/// <summary>
		/// imports all the properties of the module marked with an attribute of type A
		/// </summary>
		public static void ImportServices<A>(this IKernel kernel, object module)
			where A: Attribute
		{
			Type type = module.GetType();
			
			foreach(var prop in type.GetProperties())
				if(prop.IsDefined(typeof(A), false))
			{
				string name = InjectIdentity.GetName(prop);
				if(name == null)
					kernel.Bind(prop.PropertyType).ToConstant(prop.GetValue(module, null));
				else
					kernel.Bind(prop.PropertyType).ToConstant(prop.GetValue(module, null)).Named(name);
			}
		}
		
		public static string ShowType(Type type)
		{
			return string.Format("{0} in assembly {1}", type.FullName, type.Assembly.FullName);
		}
	}
}

