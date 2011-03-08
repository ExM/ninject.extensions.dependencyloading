using System;
using Ninject;

namespace DependencyLoadingTest
{
	public class TestProvider: ITestModule
	{
		[TestExport]
		public IServiceA ServA {get; private set;}
		
		[TestExport, Named("Name_B1")]
		public IServiceB ServB1 {get; private set;}
		
		[TestExport, Named("Name_B2")]
		public IServiceB ServB2 {get; private set;}
		
		[TestExport]
		public IServiceC ServC {get; private set;}
		
		public TestProvider()
		{
		}
		
		public void Initialize()
		{
			ServA = new ServiceARealization();
			ServB1 = new ServiceBRealization();
			ServB2 = new ServiceBRealization();
			ServC = new ServiceCRealization();
		}
	}
}

