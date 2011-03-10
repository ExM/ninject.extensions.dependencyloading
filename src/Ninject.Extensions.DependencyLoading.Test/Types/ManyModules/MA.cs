using System;
using Ninject;
using DependencyLoadingTest;

namespace t
{
	public class MA: ITestModule
	{
		[TestExport]
		public IServA ServA {get; set;}
		
		[TestExport]
		public IServB ServB {get; set;}
		
		public MA()
		{
		}
		
		public void Initialize()
		{
			ServA = new ServA();
			ServB = new ServB();
		}
	}
}

