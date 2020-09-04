using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Toolbox
{
	[CustomEditor(typeof(BlurPanel))]
	public class BlurPanelEditor : ImageEditor
	{
		private SerializedProperty isAnimated;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("blurValue"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("delay"));

			isAnimated = serializedObject.FindProperty("isAnimated");
			EditorGUILayout.PropertyField(isAnimated);
			if (isAnimated.boolValue)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("time"));
			}

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}
