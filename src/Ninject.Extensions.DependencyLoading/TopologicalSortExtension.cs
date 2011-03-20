using System;
using System.Linq;
using System.Collections.Generic;

namespace Ninject.Extensions.DependencyLoading
{
	public static class TopologicalSortExtension
	{
		public static List<T> TopologicalSort<T>(this IEnumerable<T> list, Func<T, T, bool> isDependency)
		{
			return TopologicalSort<T>(list, isDependency, (item) => item.ToString());
		}
		
		public static List<T> TopologicalSort<T>(this IEnumerable<T> list, Func<T, T, bool> isDependency, Func<T, string> showItem)
		{
			Dictionary<T, Node<T>> index = new Dictionary<T, Node<T>>();
			List<Node<T>> nodes = new List<Node<T>>();
			foreach(T item in list)
			{
				Node<T> node = new Node<T>(item);
				index.Add(item, node);
				nodes.Add(node);
			}
			foreach(Node<T> node in nodes)
			foreach(T item in list)
				if(!object.Equals(node.Id, item) && isDependency(node.Id, item))
					node.Dependencies.Add(index[item]);
			
			try
			{
				return new Sorter<T>(nodes, showItem).Result;
			}
			catch(CycleDetectException err)
			{
				throw err; // clear stacktrace
			}
		}
		
		private class Node<T>
		{
			public readonly T Id;
			public int Color = 0;
			public readonly List<Node<T>> Dependencies = new List<Node<T>>();
			public Node(T id)
			{
				Id = id;
			}
		}
		
		private class Sorter<T>
		{
			private readonly Func<T, string> _showItem;
			public readonly List<T> Result = new List<T>();
			
			public Sorter(List<Node<T>> nodes, Func<T, string> showItem)
			{
				_showItem = showItem;
				foreach(Node<T> node in nodes)
					Enter(node);
			}
			
			private bool Enter(Node<T> node)
			{
				if(node.Color == 2)
					return true;
				
				if(node.Color == 1)
					return false; // cycle detect
				
				node.Color = 1;
				
				foreach (Node<T> dependency in node.Dependencies)
					if(!Enter(dependency))
						throw new CycleDetectException(_showItem(node.Id), _showItem(dependency.Id));
				
				node.Color = 2;
				Result.Add(node.Id);
				return true;
			}
		}
	}
}

