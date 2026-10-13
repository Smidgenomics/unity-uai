// smidgens @ github

namespace Smidgenomics.Unity.UAI
{
	using UnityEngine;
	using System;
	using System.Collections.Generic;

	[Serializable]
	[UAIDebugIcon(UAIConstants.UAI_ATLAS_GUID, "56/8")]
	public abstract class UAISelector
	{
		// Assumes sorted values in descending order
		public abstract int SelectIndex(int count, Func<int, float> scoreFn);

		// assumes descending order
		protected int GetBestIndexFromSortedDesc(int count, Func<int, float> scoreFn)
		{
			for (var i = 0; i < count; i++)
			{
				var s = scoreFn.Invoke(i);
				if (Mathf.Approximately(s, 0f))
				{
					continue;
				}
				// if we're in desc order then first non-zero is best
				return i;
			}
			return -1;
		}

		protected void CollectMinScores
		(
			int count, int startIndex,
			float minScore,
			Func<int, float> scoreFn,
			out List<int> indices,
			out List<float> scores
		)
		{
			indices = new List<int>();
			scores = new List<float>();
			for (var i = startIndex; i < count; i++)
			{
				var score = scoreFn.Invoke(i);
				if (score < minScore || Mathf.Approximately(score, 0))
				{
					continue;
				}
				indices.Add(i);
				scores.Add(score);
			}
		}
	}
}