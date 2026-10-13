// smidgens @ github

namespace Smidgenomics.Unity.UAI
{
	using UnityEngine;
	using System;
	using System.ComponentModel;

	[DisplayName("Random Weighted")]
	[Serializable]
	[UAIDebugIcon(UAIConstants.UAI_ATLAS_GUID, "56/8")]
	public sealed class UAISelector_RandomWeighted : UAISelector
	{
		public override int SelectIndex(int count, Func<int, float> scoreFn)
		{
			var topIndex = GetBestIndexFromSortedDesc(count, scoreFn);
			if (topIndex < 0)
			{
				return -1;
			}
			var topScore = scoreFn.Invoke(topIndex);
			var minScore = topScore;
			CollectMinScores(count, topIndex, minScore, scoreFn, out var indices, out var scores);
			return indices[UAIMath.GetRandomArrayIndexWeighted<float>(scores)];
		}
	}
}