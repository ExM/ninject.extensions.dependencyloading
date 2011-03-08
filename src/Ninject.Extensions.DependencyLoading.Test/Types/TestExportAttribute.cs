using System;

namespace DependencyLoadingTest
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class TestExportAttribute: Attribute
	{
		public TestExportAttribute()
		{
		}
	}
}

