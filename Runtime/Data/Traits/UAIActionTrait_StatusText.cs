// smidgens @ github

namespace Smidgenomics.Unity.UAI
{
	using UnityEngine;
	using System;
	using System.ComponentModel;
	using UnityEngine.Scripting.APIUpdating;

	/// <summary>
	/// Status text describing the action
	/// </summary>
	[DisplayName("Status Text")]
	[Serializable]
	[MovedFrom(false, null, null, "UAIActionStatusText")]
	public sealed class UAIActionTrait_StatusText : UAIActionTrait
	{
		public string Text => _text;

		[SerializeField] private string _text;
	}
}