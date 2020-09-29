using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Toolbox
{
	public class TextLocaliserEditorWindow : EditorWindow
	{
		public string key = "";
		public string value = "";

		public static void Open(string key)
		{
			TextLocaliserEditorWindow window = CreateInstance<TextLocaliserEditorWindow>();
			window.titleContent = new GUIContent("Localiser window");
			window.ShowUtility();
			window.key = key;
		}

		public void OnGUI()
		{
			key = EditorGUILayout.TextField("Key : ", key);

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField("Value : ", GUILayout.MaxWidth(50.0f));
			EditorStyles.textArea.wordWrap = true;
			value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.Width(400.0f), GUILayout.Height(100.0f));

			EditorGUILayout.EndHorizontal();

			if (GUILayout.Button("Add"))
			{
				if (LocalisationSystem.GetLocalisedValue(key) != string.Empty)
				{
					LocalisationSystem.Edit(key, value);
				}
				else
				{
					LocalisationSystem.Add(key, value);
				}
			}

			minSize = new Vector2(460.0f, 250.0f);
			maxSize = minSize;
		}
	}

	public class TextLocaliserSearchWindow : EditorWindow
	{
		public string value = "";
		public Vector2 scroll = Vector2.zero;
		public Dictionary<string, string> dictionary = null;

		public static void Open()
		{
			TextLocaliserSearchWindow window = CreateInstance<TextLocaliserSearchWindow>();
			window.titleContent = new GUIContent("Localisation search");

			Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			Rect r = new Rect(mouse.x - 450.0f, mouse.y + 10.0f, 10.0f, 10.0f);
			window.ShowAsDropDown(r, new Vector2(500.0f, 300.0f));
		}

		private void OnEnable()
		{
			dictionary = LocalisationSystem.GetDictionaryForEditor(LocalisationSystem.Language.English);
		}

		private void OnGUI()
		{
			EditorGUILayout.BeginHorizontal("Box");

			EditorGUILayout.LabelField("Search : ", EditorStyles.boldLabel);
			value = EditorGUILayout.TextField(value);

			EditorGUILayout.EndHorizontal();

			GetSearchResults();
		}

		private void GetSearchResults()
		{
			EditorGUILayout.BeginVertical();

			scroll = EditorGUILayout.BeginScrollView(scroll);

			foreach (KeyValuePair<string, string> pair in dictionary)
			{
				if (pair.Key.ToLower().Contains(value.ToLower()) || pair.Value.ToLower().Contains(value.ToLower()))
				{
					EditorGUILayout.BeginHorizontal("Box");

					Texture closeIcon = Resources.Load<Texture>("close");
					GUIContent content = new GUIContent(closeIcon);

					if (GUILayout.Button(content, GUILayout.MaxWidth(20.0f), GUILayout.MaxHeight(20.0f)))
					{
						if (EditorUtility.DisplayDialog("Remove key " + pair.Key + "?", "This will remove the element from localisation, are you sure?", "Do it"))
						{
							LocalisationSystem.Remove(pair.Key);
							AssetDatabase.Refresh();

							dictionary = LocalisationSystem.GetDictionaryForEditor(LocalisationSystem.Language.English);
						}
					}

					EditorGUILayout.TextField(pair.Key);
					EditorGUILayout.LabelField(pair.Value);

					EditorGUILayout.EndHorizontal();
				}
			}

			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}
	}
}