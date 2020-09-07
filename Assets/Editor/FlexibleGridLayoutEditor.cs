using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Toolbox
{
    [CustomEditor(typeof(FlexibleGridLayout))]
    public class FlexibleGridLayoutEditor : Editor
    {
		private SerializedProperty fitType;

		public override void OnInspectorGUI()
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Padding"));

			fitType = serializedObject.FindProperty("fitType");

			EditorGUILayout.PropertyField(fitType);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("spacing"));

			if (fitType.enumValueIndex == (int)FlexibleGridLayout.FitType.FixedRows)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("rows"));

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("fitX"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("fitY"));
				EditorGUILayout.EndHorizontal();
			}
			else if (fitType.enumValueIndex == (int)FlexibleGridLayout.FitType.FixedColumns)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("columns"));

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("fitX"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("fitY"));
				EditorGUILayout.EndHorizontal();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
