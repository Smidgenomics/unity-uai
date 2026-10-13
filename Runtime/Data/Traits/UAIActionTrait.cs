// smidgens @ github

namespace Smidgenomics.Unity.UAI
{
	using UnityEngine;
	using System;

	/// <summary>
	/// A way to stick custom data/logic to action instance
	/// </summary>
	[Serializable]
	public abstract class UAIActionTrait
	{
		public string Name => _name;

		[HideInInspector]
		[SerializeField] protected string _name;
	}
}