using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject;

namespace Ninject.Extensions.DependencyLoading
{
	public static class TypeDependencyDetector<T>
		where T: Attribute
	{
		
		public static bool IsDependency(Type consumer, Type provider)
		{
			List<InjectIdentity> consumerCache = null;
			
			foreach(InjectIdentity pi in ExportedProperties(provider))
			{
				if(consumerCache == null)
				{
					consumerCache = new List<InjectIdentity>();
					foreach(InjectIdentity ci in Injection(consumer))
					{
						if(pi.Equals(ci))
							return true;
						consumerCache.Add(ci);
					}
				}
				else
				{
					foreach(InjectIdentity ci in consumerCache)
						if(pi.Equals(ci))
							return true;
				}
			}
			
			return false;
		}
		
		public static IEnumerable<InjectIdentity> Injection(Type consumer)
		{
			foreach(var prop in consumer.GetProperties())
				if(prop.IsDefined(typeof(InjectAttribute), false))
					yield return new InjectIdentity(prop);
			
			foreach(var ctor in consumer.GetConstructors())
				if(ctor.IsDefined(typeof(InjectAttribute), false))
					foreach(var pi in ctor.GetParameters())
						yield return new InjectIdentity(pi);
		}
		
		public static IEnumerable<InjectIdentity> ExportedProperties(Type provider)
		{
			foreach(var prop in provider.GetProperties())
				if(prop.IsDefined(typeof(T), false))
					yield return new InjectIdentity(prop);
		}
	}
}

