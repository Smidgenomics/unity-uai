// smidgens @ github

namespace Smidgenomics.Unity.UAI
{
	using System;
	using System.Diagnostics;
	using UnityEngine;

	/// <summary>
	/// Used by editor GUI
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[Conditional("UNITY_EDITOR")]
	internal sealed class UAIDebugIconAttribute : Attribute
	{
		public UAIDebugIconAttribute(string texRef, string uv = null)
		{
			this.texRef = texRef;
			this.uv = UAIParse.ParseRect(uv, new Rect(0, 0, 1, 1));
		}

		internal readonly string texRef;
		internal readonly Rect uv;
	}
}