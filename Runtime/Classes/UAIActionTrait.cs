// smidgens @ github

namespace Smidgenomics.Unity.UAI
{
	using UnityEngine;
	using System;
	using System.ComponentModel;

	/// <summary>
	/// A way to stick additional data on action
	/// </summary>
	[Serializable]
	[DisplayName("Action Trait")]
	public abstract class UAIActionTrait
	{
		public string Name => _name;

		[HideInInspector]
		[SerializeField] protected string _name;
	}
}

namespace Smidgenomics.Unity.UAI
{
	using UnityEngine;
	using System;
	using System.ComponentModel;

	[DisplayName("Status Text")]
	[Serializable]
	public sealed class UAIActionStatus : UAIActionTrait
	{
		public string Text => _text;

		[SerializeField] private string _text;
	}
}