using System;
using NUnit.Framework;
using System.Collections.Generic;
using Ninject.Extensions.DependencyLoading;

namespace DependencyLoadingTest
{
	[TestFixture]
	public class TopologicalSortTest
	{
		[Test]
		public void NoDependency()
		{
			List<string> list = new List<string>();
			list.Add("A");
			list.Add("B");
			list.Add("C");
			
			Func<string, string, bool> isDependency = (s1, s2) => 
			{
				return false;
			};
			
			List<string> sorted = list.TopologicalSort(isDependency);
			
			Assert.AreEqual("A", sorted[0]);
			Assert.AreEqual("B", sorted[1]);
			Assert.AreEqual("C", sorted[2]);
		}
		
		[TestCase("A","B")]
		[TestCase("B","A")]
		public void OneDependency(string i1, string i2)
		{
			List<string> list = new List<string>();
			list.Add(i1);
			list.Add(i2);
			
			Func<string, string, bool> isDependency = (s1, s2) => 
			{
				if(s1 == "B" && s2 == "A")
					return true;
				
				return false;
			};
			
			List<string> sorted = list.TopologicalSort(isDependency);
			
			Assert.AreEqual("A", sorted[0]);
			Assert.AreEqual("B", sorted[1]);
		}
		
		[TestCase(1,2,3)]
		[TestCase(2,1,3)]
		[TestCase(1,3,2)]
		[TestCase(2,3,1)]
		[TestCase(3,1,2)]
		[TestCase(3,2,1)]
		public void MoreDependency(int i1, int i2, int i3)
		{
			List<int> list = new List<int>();
			list.Add(i1);
			list.Add(i2);
			list.Add(i3);
			
			Func<int, int, bool> isDependency = (s1, s2) => 
			{
				if(s1 == 1 && s2 == 2) return true;
				if(s1 == 2 && s2 == 3) return true;
				return false;
			};
			
			List<int> sorted = list.TopologicalSort(isDependency);
			
			Assert.AreEqual(3, sorted[0]);
			Assert.AreEqual(2, sorted[1]);
			Assert.AreEqual(1, sorted[2]);
		}
		
		[Test]
		public void ManyDependency()
		{
			List<string> list = new List<string>(new string[]{
				"A", "B", "C", "D", "E", "F", "G"
			});
			
			List<string> dep = new List<string>(new string[]{
				"AB", "AC", "DB", "DE", "CF", "BF", "FG", "EG"
			});
			
			Func<string, string, bool> isDependency = (s1, s2) => 
			{
				return dep.Contains(s1 + s2);
			};
			
			List<string> sorted = list.TopologicalSort(isDependency);
			
			Assert.AreEqual("G", sorted[0]);
		}
		
		[TestCase(1,2,3,4)]
		[TestCase(4,3,2,1)]
		[TestCase(1,4,3,2)]
		[TestCase(3,4,1,2)]
		public void MoreDependency(int i1, int i2, int i3, int i4)
		{
			List<int> list = new List<int>();
			list.Add(i1);
			list.Add(i2);
			list.Add(i3);
			list.Add(i4);
			
			Func<int, int, bool> isDependency = (s1, s2) => 
			{
				if(s1 == 1 && s2 == 2) return true;
				if(s1 == 1 && s2 == 3) return true;
				if(s1 == 2 && s2 == 4) return true;
				if(s1 == 3 && s2 == 4) return true;
				return false;
			};
			
			List<int> sorted = list.TopologicalSort(isDependency);
			
			Assert.AreEqual(4, sorted[0]);
			Assert.AreEqual(1, sorted[3]);
		}
		
		[Test]
		[ExpectedException(typeof(CycleDetectException))]
		public void CycleDetect()
		{
			List<string> list = new List<string>();
			list.Add("A");
			list.Add("B");
			list.Add("C");
			
			Func<string, string, bool> isDependency = (s1, s2) => 
			{
				if(s1 == "A" && s2 == "B") return true;
				if(s1 == "B" && s2 == "C") return true;
				if(s1 == "C" && s2 == "A") return true;
				
				return false;
			};
			
			list.TopologicalSort(isDependency);
		}
	}
}

