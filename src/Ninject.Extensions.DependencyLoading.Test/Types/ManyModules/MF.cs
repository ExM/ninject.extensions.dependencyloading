using System;
using Ninject;
using DependencyLoadingTest;

namespace t
{
	public class MF: ITestModule
	{
		[Inject]
		public IServE ServE {get; set;}
		
		[Inject]
		public IServF ServF {get; set;}
		
		[TestExport]
		public IServG ServG {get; set;}
		
		public MF()
		{
		}
		
		public void Initialize()
		{
			ServG = new ServG();
		}
	}
}

