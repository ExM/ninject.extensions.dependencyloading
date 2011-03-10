using System;
using Ninject;
using DependencyLoadingTest;

namespace t
{
	public class MC: ITestModule
	{
		[TestExport]
		public IServF ServF {get; set;}
		
		[Inject]
		public IServA ServA {get; set;}
		
		public MC()
		{
		}
		
		public void Initialize()
		{
			ServF = new ServF();
		}
	}
}

