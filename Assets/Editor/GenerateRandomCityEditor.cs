using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Toolbox
{
	[CustomEditor(typeof(GenerateCity))]
	public class GenerateRandomCityEditor : Editor
	{
		private GenerateCity city = null;

		private void Awake()
		{
			city = serializedObject.targetObject as GenerateCity;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUILayout.Space();

			if (GUILayout.Button("Generate"))
			{
				city.Generate();
			}

			EditorGUILayout.Space();

			if (GUILayout.Button("Clear"))
			{
				city.Clear();
			}
		}
	}
}
