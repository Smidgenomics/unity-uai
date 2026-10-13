// smidgens @ github

// ReSharper disable VirtualMemberNeverOverridden.Global
#pragma warning disable 0414
#pragma warning disable 0067

namespace Smidgenomics.Unity.UAI
{
	using UnityEngine;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using IEnumerator = System.Collections.IEnumerator;

	// action base
	[UAIListField(nameof(_weight), 30f)]
	public abstract class UAIAction :  UAINode, IUAIAction
	{
		/// <summary>
		/// Activation status in UAI brain
		/// </summary>
		public EUAIActionStatus GetActionStatus()
		{
			return _status;
		}

		public T GetActionTrait<T>() where T : UAIActionTrait
		{
			foreach (var t in _traits)
			{
				if (t is T found)
				{
					return found;
				}
			}
			return null;
		}

		/// <summary>
		/// Optional multiplier for weight, can be used as extra guard for prerequisites before trying to activate
		/// </summary>
		public virtual float GetWeightModifier()
		{
			return 1f;
		}

		/// <summary>
		/// Returns wait time before action can be considered again after deactivation
		/// </summary>
		public virtual float GetActionCooldown()
		{
			return _defaultCooldown;
		}

		/// <summary>
		/// Run the entirety of an action's logic
		/// </summary>
		public virtual IEnumerator ActivateAction()
		{
			return null;
		}

		/// <summary>
		/// Deactivation routine, will always run to completion
		/// </summary>
		public virtual IEnumerator DeactivateAction()
		{
			return null;
		}

		/// <summary>
		/// When true, action can be cancelled if another is selected
		/// </summary>
		public virtual bool CanCancelAction()
		{
			return true;
		}

		public virtual bool IsReusable()
		{
			return true;
		}

		// public string GetStatusText()
		// {
		// 	return _statusText;
		// }

		protected void FinishAction()
		{
			FinishWithStatus(EUAIActionStatus.Finished);
		}

		internal float GetActionWeight()
		{
			return _weight * Mathf.Max(GetWeightModifier(), 0f);
		}

		internal void CancelAction()
		{
			FinishWithStatus(EUAIActionStatus.Cancelled);
		}

		internal UAIAction InstantiateAction()
		{
			var instance = Instantiate(this);
			instance.name = name;
			return instance;
		}

		internal Action onActionFinished;

		[HideInInspector]
		[SerializeField] internal string _statusText = string.Empty;

		[Min(0f)]
		[HideInInspector]
		[SerializeField] internal float _weight = 1f;
		
		/// <summary>
		/// When enabled, the action will not be re-scored while it's active (it sustains the same score)
		/// </summary>
		[HideInInspector]
		[SerializeField] internal bool _sustainAction;

		[EditConditionToggle(nameof(_sustainAction), "Sustain Action")]
		// [UAICurve(x:0f, y:0f, w:10f, h:5f)]
		[SerializeField] internal AnimationCurve _sustainCurve = AnimationCurve.Linear(0, 1, 1, 1);

		// [HideInInspector]
		[UAIHideOnOverride(nameof(GetActionCooldown))]
		[SerializeField,Min(0f)] private float _defaultCooldown = UAIDefaults.DEFAULT_ACTION_COOLDOWN;

		[HideInInspector]
		[SerializeField] internal SOArray<UAIConsideration> _considerations = new();

		[HideInInspector]
		[UAIInstancedReference(typeUnique:true)]
		[SerializeReference]
		[SerializeField] internal UAIActionTrait[] _traits = Array.Empty<UAIActionTrait>();

		internal EUAIActionStatus _status = EUAIActionStatus.Inactive;

		private void FinishWithStatus(EUAIActionStatus status)
		{
			_status = status;
			Action ev = onActionFinished;
			onActionFinished = null;
			ev?.Invoke();
		}

		#if UNITY_EDITOR
		// editor/inspector convenience
		internal override void GatherNestedAssets(List<UAIScriptableObject> assets)
		{
			foreach (var c in _considerations.GetArr())
			{
				if (c.item)
				{
					assets.Add(c.item);
				}
			}
		}
		#endif
	}
}

#if UNITY_EDITOR

namespace Smidgenomics.Unity.UAI.Editor
{
	using System;
	using UnityEngine;
	using UnityEditor;
	using UnityEditorInternal;

	[CustomEditor(typeof(UAIAction), true)]
	internal sealed class _UAIAction : _UAIScriptableObject
	{
		protected override void OnAfterFields()
		{
			DrawDivider();
			serializedObject.UpdateIfRequiredOrScript();
			_traitList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
			
			serializedObject.UpdateIfRequiredOrScript();
			_considerationView.OnListGUI();
			serializedObject.ApplyModifiedProperties();
		}

		private NestedAssetList _considerationView;
		private ReorderableList _traitList;
		private SerializedProperty _defaultCooldown;
		private GUIContent _foldoutLabel;

		private static readonly Lazy<GUIContent> _ICO_ADD = GetLazyUnityIcon("Toolbar Plus More");
		private static readonly Lazy<GUIContent> _ICO_RM = GetLazyUnityIcon("Toolbar Minus");
		
		private static Lazy<GUIContent> GetLazyUnityIcon(string name)
		{
			return new Lazy<GUIContent>(() => EditorGUIUtility.IconContent(name));
		}

		protected override void OnInit()
		{
			var listProp = serializedObject.FindProperty(nameof(UAIAction._considerations));
			_considerationView = CreateConsiderationList(listProp);

			var traitArr = serializedObject.FindProperty(nameof(UAIAction._traits));
			_traitList = new ReorderableList(serializedObject, traitArr)
			{
				displayAdd = false,
				displayRemove = false,
				elementHeightCallback = i =>
				{
					var item = traitArr.GetArrayElementAtIndex(i);
					return EditorGUI.GetPropertyHeight(item, GUIContent.none);
				},
				drawElementCallback = (r, i, f, a) =>
				{
					var item = traitArr.GetArrayElementAtIndex(i);
					EditorGUI.PropertyField(r, item, GUIContent.none);
				}
			};

			_traitList.drawHeaderCallback = r =>
			{
				var btnWidth = EditorStyles.iconButton.CalcSize(_ICO_ADD.Value).x;

				var rmRect = r.SliceRight(btnWidth);
				var addRect = r.SliceRight(btnWidth);

				if (GUI.Button(addRect, _ICO_ADD.Value, EditorStyles.iconButton))
				{
					var dropdown = UAIEditorUtils.CreateTypeDropdown(typeof(UAIActionTrait), t =>
					{
						for (int i = 0; i < traitArr.arraySize; i++)
						{
							var aType = traitArr.GetArrayElementAtIndex(i).managedReferenceValue?.GetType();
							if (aType == t)
							{
								return false;
							}
						}
						return true;
					}, newType =>
					{
						traitArr.arraySize++;
						var newItem = traitArr.GetArrayElementAtIndex(traitArr.arraySize - 1);
						newItem.managedReferenceValue = Activator.CreateInstance(newType);
						serializedObject.ApplyModifiedProperties();

					}, null);
					
					dropdown.Show(addRect, 400f);
					
					
					
				}
				
				if (GUI.Button(rmRect, _ICO_RM.Value, EditorStyles.iconButton))
				{
					if (_traitList.index < 0 || _traitList.index >= traitArr.arraySize)
					{
						return;
					}
					traitArr.DeleteArrayElementAtIndex(_traitList.index);
				}
				GUI.Label(r, traitArr.displayName);
			};

		}

		private void OnDisable()
		{
			_considerationView?.DisposeGUI();
		}

		private static NestedAssetList CreateConsiderationList(SerializedProperty listProp)
		{
			return new (listProp, typeof(UAIConsideration))
			{
				HeaderLabel = "Action Considerations",
				EmptyText = "List empty, base score will default to 1.",
				HeaderIcon = EUAIAtlasIcon.Consideration
			};
		}
		
	}
}

#endif