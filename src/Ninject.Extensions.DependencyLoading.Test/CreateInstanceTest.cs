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
	public class CreateInstanceTest
	{
		[Test]
		public void Summary()
		{
			List<Type> modules = new List<Type>{
				typeof(TestConsumer),
				typeof(TestProvider)};
			
			List<Type> sorted = modules.Sort<TestExportAttribute>();
			
			Assert.AreEqual(typeof(TestProvider), sorted[0]);
			Assert.AreEqual(typeof(TestConsumer), sorted[1]);
			
			IKernel kernel = new StandardKernel();
			kernel.LoadModules<ITestModule>(sorted, (module) =>
			{
				module.Initialize();
				kernel.ImportServices<TestExportAttribute>(module);
			});
		}
		
		[Test]
		public void Many()
		{
			List<Type> modules = new List<Type>{
				typeof(MG),
				typeof(MF),
				typeof(ME),
				typeof(MD),
				typeof(MC),
				typeof(MB),
				typeof(MA)};
			
			List<Type> sorted = modules.Sort<TestExportAttribute>();
			
			Assert.AreEqual(typeof(MG), sorted[6]);
			
			IKernel kernel = new StandardKernel();
			kernel.LoadModules<ITestModule>(sorted, (module) =>
			{
				module.Initialize();
				kernel.ImportServices<TestExportAttribute>(module);
			});
		}
		
		[Test]
		public void ManyWarning()
		{
			List<Type> modules = new List<Type>{
				typeof(MG),
				typeof(MF),
				typeof(ME),
				typeof(MD),
				typeof(MC),
				typeof(MB),
				typeof(MA)};
			
			List<Type> warning = null;
			IKernel kernel = new StandardKernel();
			kernel.LoadModules<ITestModule>(modules, (module) =>
			{
				module.Initialize();
				kernel.ImportServices<TestExportAttribute>(module);
			},
			(list) => {warning = list;}, (list) => {});
			
			Assert.IsNotNull(warning);
			
			Assert.AreEqual(typeof(MG), warning[6]);
		}
		
		[Test]
		public void ManyErrorOne()
		{
			List<Type> modules = new List<Type>{
				typeof(MG),
				typeof(ME),
				typeof(MD),
				typeof(MC),
				typeof(MB),
				typeof(MA)};
			
			List<Type> error = null;
			IKernel kernel = new StandardKernel();
			kernel.LoadModules<ITestModule>(modules, (module) =>
			{
				module.Initialize();
				kernel.ImportServices<TestExportAttribute>(module);
			},
			(list) => {}, (list) => {error = list;});
			
			Assert.IsNotNull(error);
			Assert.AreEqual(1, error.Count);
			Assert.AreEqual(typeof(MG), error[0]);
		}
		
		[Test]
		public void ManyError3()
		{
			List<Type> modules = new List<Type>{
				typeof(MG),
				typeof(MF),
				typeof(ME),
				typeof(MC),
				typeof(MB),
				typeof(MA)};
			
			List<Type> error = null;
			IKernel kernel = new StandardKernel();
			kernel.LoadModules<ITestModule>(modules, (module) =>
			{
				module.Initialize();
				kernel.ImportServices<TestExportAttribute>(module);
			},
			(list) => {}, (list) => {error = list;});
			
			Assert.IsNotNull(error);
			Assert.AreEqual(4, error.Count);
			Assert.IsTrue(error.Contains(typeof(MG)));
			Assert.IsTrue(error.Contains(typeof(MF)));
			Assert.IsTrue(error.Contains(typeof(ME)));
			Assert.IsTrue(error.Contains(typeof(MB)));
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

