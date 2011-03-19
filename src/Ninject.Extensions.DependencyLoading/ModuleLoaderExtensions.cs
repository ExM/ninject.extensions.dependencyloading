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
			List<Type> remain = modules.ToList();
			List<Type> postponed = new List<Type>();
			
			bool isWarn = false;
			while(true)
			{
				foreach(Type type in remain)
				{
					object instance = (T)kernel.TryGet(type);
					if(instance == null)
					{
						postponed.Add(type);
						continue;
					}
					else
					{
						T module = (T)instance;
						initialization(module);
						sorted.Add(type);
					}
				}
				
				if(postponed.Count == remain.Count)
				{
					error(postponed);
					return;
				}
				
				if(postponed.Count == 0)
				{
					if(isWarn)
						warning(sorted);
					return;
				}
				
				isWarn = true;
				remain = postponed;
			}
		}
		
		public static List<Type> Sort<A>(this IEnumerable<Type> modules)
			where A: Attribute
		{
			return modules.TopologicalSort(TypeDependencyDetector<A>.IsDependency, ShowType);
		}
		
		public static void LoadModules<T>(this IKernel kernel, IEnumerable<Type> modules, Action<T> initialization)
		{
			foreach(Type type in modules)
			{
				T module = (T)kernel.Get(type);
				initialization(module);
			}
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

