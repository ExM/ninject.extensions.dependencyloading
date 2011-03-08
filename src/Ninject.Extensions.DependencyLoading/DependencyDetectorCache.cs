using System;
using System.Collections.Generic;

namespace Ninject.Extensions.DependencyLoading
{
	public class DependencyDetectorCache<T>
	{
		Dictionary<T, Dictionary<T, bool>> _cache = new Dictionary<T, Dictionary<T, bool>>();
		private Func<T, T, bool> _isDependency;
		
		public DependencyDetectorCache(Func<T, T, bool> isDependency)
		{
			_isDependency = isDependency;
		}
		
		public bool IsDependency(T item1, T item2)
		{
			Dictionary<T, bool> cache2;
			bool result;
			
			if(!_cache.TryGetValue(item1, out cache2))
			{
				cache2 = new Dictionary<T, bool>();
				_cache.Add(item1, cache2);
				
				result = _isDependency(item1, item2);
				cache2.Add(item2, result);
			}
			else
			{
				if(!cache2.TryGetValue(item2, out result))
				{
					result = _isDependency(item1, item2);
					cache2.Add(item2, result);
				}
			}
			
			return result;
		}
	}
}

