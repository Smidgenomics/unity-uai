// smidgens @ github

namespace Smidgenomics.Unity.UAI
{
	using UnityEngine;
	using System;
	using System.ComponentModel;

	[DisplayName("Top Score %")]
	[Serializable]
	[UAIDebugIcon(UAIConstants.UAI_ATLAS_GUID, "58/8")]
	public sealed class UAISelector_TopScoreInterval : UAISelector
	{
		public override int SelectIndex(int count, Func<int, float> scoreFn)
		{
			var topIndex = GetBestIndexFromSortedDesc(count, scoreFn);

			if (topIndex < 0)
			{
				return -1;
			}
			var topScore = scoreFn.Invoke(topIndex);
			var minScore = topScore - (topScore * _interval);
			CollectMinScores(count, topIndex, minScore, scoreFn, out var indices, out var scores);
			if (indices.Count == 1)
			{
				return topIndex;
			}
			if (_randomMethod == EUAIRandomMethod.Weighted)
			{
				return indices[UAIMath.GetRandomArrayIndexWeighted<float>(scores)];
			}
			return indices[UnityEngine.Random.Range(0, indices.Count - 1)];
		}

		[Range(0, 1)]
		[SerializeField] private float _interval = 0.25f;

		[SerializeField] private EUAIRandomMethod _randomMethod = EUAIRandomMethod.Uniform;
	}
}