using System;
using Ninject;
using DependencyLoadingTest;

namespace t
{
	public class MB: ITestModule
	{
		[Inject]
		public IServB ServB {get; set;}
		
		[Inject]
		public IServC ServC {get; set;}
		
		[TestExport]
		public IServE ServE {get; set;}
		
		public MB()
		{
		}
		
		public void Initialize()
		{
			ServE = new ServE();
		}
	}
}

