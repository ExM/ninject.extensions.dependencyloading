using System;

namespace Ninject.Extensions.DependencyLoading
{
	/// <summary>
	/// Cycle detect
	/// </summary>
	public class CycleDetectException: ApplicationException
	{
		/// <summary>
		/// Cycle detect
		/// </summary>
		public CycleDetectException(string item1, string item2)
			:base(string.Format("Cycle detect. Reason: `{0}' required `{1}'.", item1, item2))
		{
		}
	}
}

