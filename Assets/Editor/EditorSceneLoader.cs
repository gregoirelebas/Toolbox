using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorSceneLoader : EditorWindow
{
	private static EditorSceneLoader window = null;

	private Vector2 scroll = Vector2.zero;
	private bool loadScene = true;

	[MenuItem("Window/SceneLoader")]
	private static void LoadWindow()
	{
		window = GetWindow<EditorSceneLoader>("Scene loader", true);
	}

	public void OnGUI()
	{
		scroll = EditorGUILayout.BeginScrollView(scroll);

		if (loadScene)
		{
			if (GUILayout.Button("Load/Unload : YES"))
			{
				loadScene = false;
			}
		}
		else
		{
			if (GUILayout.Button("Load/Unload : NO"))
			{
				loadScene = true;
			}
		}

		GUILayout.Space(20.0f);

		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
		{
			string[] splittedPath = scene.path.Split(new char[] { '/' });

			//Our scene name is at the end of the path
			string name = splittedPath[splittedPath.Length - 1];
			name = name.Replace(".unity", "");

			Color oldColor = GUI.color;

			if (name.ToLower().Equals(SceneManager.GetActiveScene().name.ToLower()))
			{
				GUI.color = Application.isPlaying ? Color.yellow : Color.green;
			}

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(name, GUILayout.Width(position.width * 0.75f)))
			{
				EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

				EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
			}

			if (EditorBuildSettings.scenes.Length > 1)
			{
				bool isActive = false;
				int loadSceneCount = 0;

				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					Scene currentScene = SceneManager.GetSceneAt(i);

					if (name.ToLower().Equals(currentScene.name.ToLower()))
					{
						isActive = true;
					}

					if (currentScene.isLoaded)
					{
						loadSceneCount++;
					}					
				}

				if (isActive)
				{
					GUI.color = Color.red;

					if (GUILayout.Button(loadScene ? "Sub" : "Unload", GUILayout.Width(position.width * 0.25f)))
					{
						if ((SceneManager.sceneCount > 1  && loadScene) || (loadSceneCount > 1 && !loadScene))
						{
							EditorSceneManager.CloseScene(SceneManager.GetSceneByName(name), loadScene);
						}
					}
				}
				else
				{
					GUI.color = oldColor;

					if (GUILayout.Button(loadScene ? "Add" : "Load", GUILayout.Width(position.width * 0.25f)))
					{
						EditorSceneManager.OpenScene(scene.path, loadScene ? OpenSceneMode.Additive : OpenSceneMode.AdditiveWithoutLoading);
					}
				}
			}

			GUILayout.EndHorizontal();

			GUI.color = oldColor;
		}

		EditorGUILayout.EndScrollView();
	}
}
