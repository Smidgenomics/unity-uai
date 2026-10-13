// smidgens @ github

namespace Smidgenomics.Unity.UAI
{
	using System;
	using System.ComponentModel;

	[DisplayName("Top Score")]
	[Serializable]
	[UAIDebugIcon(UAIConstants.UAI_ATLAS_GUID, "57/8")]
	public sealed class UAISelector_TopScore : UAISelector
	{
		public override int SelectIndex(int count, Func<int, float> scoreFn)
		{
			return GetBestIndexFromSortedDesc(count, scoreFn);
		}
	}
}