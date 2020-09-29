using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(LocalisedString))]
public class LocalisedStringDrawer : PropertyDrawer
{
    private bool dropDown = false;
    private float height = 100.0f;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (dropDown)
		{
			return height + 25.0f;
		}

		return 20.0f;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		position.width -= 34.0f;
		position.height = 18.0f;

		Rect valueRect = new Rect(position);
		valueRect.x += 15.0f;
		valueRect.width -= 15.0f;

		Rect foldButtonRect = new Rect(position);
		foldButtonRect.width = 15.0f;

		dropDown = EditorGUI.Foldout(foldButtonRect, dropDown, "");

		position.x += 15.0f;
		position.width -= 15.0f;

		SerializedProperty key = property.FindPropertyRelative("key");
		key.stringValue = EditorGUI.TextField(position, key.stringValue);

		position.x += position.width + 2.0f;
		position.width = 17.0f;
		position.height = 17.0f;

		Texture searchIcon = Resources.Load<Texture>("Icons/search");
		GUIContent searchContent = new GUIContent(searchIcon);

		if (GUI.Button(position, searchContent))
		{
			TextLocaliserSearchWindow.Open();
		}

		position.x += position.width + 2.0f;

		Texture addIcon = Resources.Load<Texture>("Icons/add");
		GUIContent addContent = new GUIContent(addIcon);

		if (GUI.Button(position, addContent))
		{
			TextLocaliserEditorWindow.Open(key.stringValue);
		}

		if (dropDown)
		{
			string value = LocalisationSystem.GetLocalisedValue(key.stringValue);

			GUIStyle style = GUI.skin.box;
			height = style.CalcHeight(new GUIContent(value), valueRect.width);

			valueRect.height = height;
			valueRect.y += 21.0f;

			EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);
		}

		EditorGUI.EndProperty();
	}
}
