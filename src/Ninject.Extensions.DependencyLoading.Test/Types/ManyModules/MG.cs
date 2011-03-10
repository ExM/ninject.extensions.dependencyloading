using System;
using Ninject;
using DependencyLoadingTest;

namespace t
{
	public class MG: ITestModule
	{
		[Inject]
		public IServG ServG {get; set;}
		
		[Inject]
		public IServH ServH {get; set;}
		
		public MG()
		{
		}
		
		public void Initialize()
		{
		}
	}
}

