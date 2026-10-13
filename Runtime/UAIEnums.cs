// smidgens @ github

namespace Smidgenomics.Unity.UAI
{
	public enum EUAIActionStatus
	{
		Inactive,
		/// <summary>
		/// Currently running
		/// </summary>
		Active,
		/// <summary>
		/// Previously running and finished
		/// </summary>
		Finished,
		/// <summary>
		/// Previously running but cancelled
		/// </summary>
		Cancelled
	}
}

namespace Smidgenomics.Unity.UAI
{
	public enum EUAIRandomMethod
	{
		/// <summary>
		/// Options have equal weights
		/// </summary>
		Uniform,
		/// <summary>
		/// Options have varying weights
		/// </summary>
		Weighted
	}
}