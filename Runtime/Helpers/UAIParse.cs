// smidgens @ github

namespace Smidgenomics.Unity.UAI
{
	using System;
	using UnityEngine;

	internal static class UAIParse
	{
		public static Rect ParseRect(string str, Rect defValue)
		{
			if (string.IsNullOrEmpty(str))
			{
				return defValue;
			}

			//  atlas position -> uv (index/x cells/y cells) or (index/cells)
			if (str.Contains('/'))
			{
				var segs = str.Split('/');
				if (segs.Length is > 3 or < 2)
				{
					return defValue;
				}
				var i = Mathf.Max(ParseInt(segs[0], 0), 0);
				var cellsX = Mathf.Max(ParseInt(segs[1], 1), 1);
				var cellsY = segs.Length > 2 ? Mathf.Max(ParseInt(segs[2], 1), 1) : cellsX;
				int y = i / cellsX;
				int x = i % cellsY;
				var w = 1f / cellsX;
				var h = 1f / cellsX;
				return new Rect(x * w, y * h, w, h);
			}

			var vals = str.Split(',');

			// offset,size
			if (vals.Length == 2)
			{
				var xy = ParseFloat(vals[0], defValue.x);
				var size = ParseFloat(vals[1], defValue.width);
				return new Rect(xy, xy, size, size);
			}

			// x,y,w,h
			if (vals.Length == 4)
			{
				Rect outVal = default;
				outVal.x = ParseFloat(vals[0], defValue.x);
				outVal.y = ParseFloat(vals[1], defValue.y);
				outVal.width = ParseFloat(vals[2], defValue.width);
				outVal.height = ParseFloat(vals[3], defValue.height);
				return outVal;
			}
			return defValue;
		}
		
		public static float ParseFloat(string str, float defValue)
		{
			return float.TryParse(str.Trim(), out var outVal)
			? outVal
			: defValue;
		}
		
		public static int ParseInt(string str, int defValue)
		{
			return int.TryParse(str.Trim(), out var outVal)
			? outVal
			: defValue;
		}
	}
}