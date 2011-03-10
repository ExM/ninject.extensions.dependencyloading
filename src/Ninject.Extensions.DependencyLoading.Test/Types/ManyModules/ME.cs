using System;
using Ninject;
using DependencyLoadingTest;

namespace t
{
	public class ME: ITestModule
	{
		[Inject]
		public IServD ServD {get; set;}
		
		[TestExport]
		public IServH ServH {get; set;}
		
		public ME()
		{
		}
		
		public void Initialize()
		{
			ServH = new ServH();
		}
	}
}

