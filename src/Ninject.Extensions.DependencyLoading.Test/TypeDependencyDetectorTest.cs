using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using Ninject.Extensions.DependencyLoading;
using t;

namespace DependencyLoadingTest
{
	[TestFixture]
	public class TyepDependencyDetectorTest
	{
		[Test]
		public void ExportedProperty()
		{
			List<InjectIdentity> expPrs = TypeDependencyDetector<TestExportAttribute>
				.ExportedProperties(typeof(TestProvider)).ToList();
			
			Assert.AreEqual(null, expPrs[0].Name);
			Assert.AreEqual(typeof(IServiceA), expPrs[0].Type);
			
			Assert.AreEqual("Name_B1", expPrs[1].Name);
			Assert.AreEqual(typeof(IServiceB), expPrs[1].Type);
			
			Assert.AreEqual("Name_B2", expPrs[2].Name);
			Assert.AreEqual(typeof(IServiceB), expPrs[2].Type);
			
			Assert.AreEqual(null, expPrs[3].Name);
			Assert.AreEqual(typeof(IServiceC), expPrs[3].Type);
		}
		
		[Test]
		public void EmptyExportedProperty()
		{
			List<InjectIdentity> expPrs = TypeDependencyDetector<TestExportAttribute>
				.ExportedProperties(typeof(TestConsumer)).ToList();
			
			Assert.AreEqual(0, expPrs.Count);
		}
		
		[Test]
		public void Injection()
		{
			List<InjectIdentity> cons = TypeDependencyDetector<TestExportAttribute>
				.Injection(typeof(TestConsumer)).ToList();
			
			Assert.AreEqual(null, cons[0].Name);
			Assert.AreEqual(typeof(IServiceA), cons[0].Type);
			
			Assert.AreEqual("Name_B1", cons[1].Name);
			Assert.AreEqual(typeof(IServiceB), cons[1].Type);
			
			Assert.AreEqual("Name_B2", cons[2].Name);
			Assert.AreEqual(typeof(IServiceB), cons[2].Type);
			
			Assert.AreEqual(null, cons[3].Name);
			Assert.AreEqual(typeof(IServiceC), cons[3].Type);
		}
		
		[Test]
		public void EmptyInjection()
		{
			List<InjectIdentity> cons = TypeDependencyDetector<TestExportAttribute>
				.Injection(typeof(TestProvider)).ToList();
			
			Assert.AreEqual(0, cons.Count);
		}
		
		[Test]
		public void Dependency()
		{
			Assert.IsTrue(TypeDependencyDetector<TestExportAttribute>
				.IsDependency(typeof(TestConsumer), typeof(TestProvider)));
			
			Assert.IsFalse(TypeDependencyDetector<TestExportAttribute>
				.IsDependency(typeof(TestProvider), typeof(TestConsumer)));
		}
		
		[TestCase(typeof(MG), typeof(MF),true)]
		[TestCase(typeof(MG), typeof(ME),true)]
		[TestCase(typeof(MF), typeof(MB),true)]
		[TestCase(typeof(MC), typeof(ME),false)]
		[TestCase(typeof(MC), typeof(MD),false)]
		[TestCase(typeof(MD), typeof(MC),false)]
		[TestCase(typeof(MA), typeof(MG),false)]
		public void Dependency(Type cons, Type prov, bool dep)
		{
			Assert.AreEqual(dep, TypeDependencyDetector<TestExportAttribute>
				.IsDependency(cons, prov));
		}
	}
}

