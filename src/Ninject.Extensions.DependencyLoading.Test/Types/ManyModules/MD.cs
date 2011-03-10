using System;
using Ninject;
using DependencyLoadingTest;

namespace t
{
	public class MD: ITestModule
	{
		[TestExport]
		public IServC ServC {get; set;}
		
		[TestExport]
		public IServD ServD {get; set;}
		
		public MD()
		{
		}
		
		public void Initialize()
		{
			ServC = new ServC();
			ServD = new ServD();
		}
	}
}

