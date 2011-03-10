using System;
using System.Reflection;
using Ninject; 

namespace Ninject.Extensions.DependencyLoading
{
	public class InjectIdentity
	{
		public string Name {get; private set;}
		public Type Type {get; private set;}
		
		public InjectIdentity(PropertyInfo pi)
		{
			Type = pi.PropertyType;
			Name = GetName(pi);
		}
		
		public InjectIdentity(ParameterInfo pi)
		{
			Type = pi.ParameterType;
			Name = GetName(pi);
		}
		
		public static string GetName(ParameterInfo pi)
		{
			object[] attrs = pi.GetCustomAttributes(typeof(NamedAttribute), false);
			if(attrs.Length == 0)
				return null;
			return ((NamedAttribute)attrs[0]).Name;
		}
		
		public static string GetName(PropertyInfo pi)
		{
			object[] attrs = pi.GetCustomAttributes(typeof(NamedAttribute), false);
			if(attrs.Length == 0)
				return null;
			return ((NamedAttribute)attrs[0]).Name;
		}
		
		public bool Equals(InjectIdentity other)
		{
			return other != null && Name == other.Name && Type == other.Type;
		}
		
		public override bool Equals (object obj)
		{
			return Equals(obj as InjectIdentity);
		}
		
		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ Type.GetHashCode();
		}
		
		public override string ToString ()
		{
			return string.Format ("[InjectIdentity: Name={0}, Type={1}]", Name, Type);
		}
	}
}

