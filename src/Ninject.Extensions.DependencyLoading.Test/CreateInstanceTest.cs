using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using Ninject.Extensions.DependencyLoading;
using Ninject;
using t;

namespace DependencyLoadingTest
{
	[TestFixture]
	public class CreateInstance
	{
		[Test]
		public void Summary()
		{
			IKernel kernel = new StandardKernel();
			
			List<ITestModule> modules = kernel.LoadModules<ITestModule, TestExportAttribute>(
				(module) => {module.Initialize();},
				typeof(TestConsumer),
				typeof(TestProvider));
			
			Assert.IsInstanceOf<TestProvider>(modules[0]);
			Assert.IsInstanceOf<TestConsumer>(modules[1]);
		}
		
		[Test]
		public void Many()
		{
			IKernel kernel = new StandardKernel();
			
			List<ITestModule> modules = kernel.LoadModules<ITestModule, TestExportAttribute>(
				(module) => {module.Initialize();},
				typeof(MG),
				typeof(MF),
				typeof(ME),
				typeof(MD),
				typeof(MC),
				typeof(MB),
				typeof(MA));
			
			Assert.IsInstanceOf<MG>(modules[6]);
		}
		
		[Test]
		public void ExportService()
		{
			IKernel kernel = new StandardKernel();
			
			TestProvider provider = new TestProvider();
			provider.Initialize();
			
			kernel.ImportServices<TestExportAttribute>(provider);
			
			Assert.IsNotNull(kernel.Get<IServiceA>());
			Assert.IsNotNull(kernel.Get<IServiceB>("Name_B1"));
			Assert.IsNotNull(kernel.Get<IServiceB>("Name_B2"));
			Assert.IsNotNull(kernel.Get<IServiceC>());
		}
		
		[Test]
		public void InjectService()
		{
			IKernel kernel = new StandardKernel();
			
			TestProvider provider = new TestProvider();
			provider.Initialize();
			
			kernel.ImportServices<TestExportAttribute>(provider);
			
			object obj = kernel.Get(typeof(TestConsumer));
			Assert.IsNotNull(obj);
		}
	}
}

