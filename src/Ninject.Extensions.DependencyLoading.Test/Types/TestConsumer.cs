using System;
using Ninject;

namespace DependencyLoadingTest
{
	public class TestConsumer: ITestModule
	{
		[Inject]
		public IServiceA ServA {get; set;}
		
		[Inject, Named("Name_B1")]
		public IServiceB ServB1 {get; set;}
		
		public IServiceB ServB2 {get; private set;}
		
		public IServiceC ServC {get; private set;}
		
		[Inject]
		public TestConsumer([Named("Name_B2")]IServiceB servB2, IServiceC servC)
		{
			ServB2 = servB2;
			ServC = servC;
		}
		
		public void Initialize()
		{
			
		}
	}
}

