using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Toolbox
{
	public class CSVLoader
	{
		private TextAsset csvFile = null;
		private char lineSeparator = '\n';
		private char surround = '"';
		private char fieldSeparator = ',';

		private string filePath = "";

		public void LoadCSVFile()
		{
			filePath = "Assets/Resources/Localisation/textLocalisation.csv";
			csvFile = Resources.Load<TextAsset>("Localisation/textLocalisation");

			if (csvFile == null)
			{
				Debug.LogError("Error trying to load " + filePath + ", file not found.");
			}
		}

		public Dictionary<string, string> GetDictionaryValues(string attributeID)
		{
			if (csvFile == null) return null;

			Dictionary<string, string> dictionary = new Dictionary<string, string>();

			string[] lines = csvFile.text.Split(lineSeparator);

			int attributeIndex = -1;

			string[] headers = lines[0].Split(fieldSeparator);
			for (int i = 0; i < headers.Length; i++)
			{
				if (headers[i].Contains(attributeID))
				{
					attributeIndex = i;
					break;
				}
			}

			Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

			for (int i = 1; i < lines.Length; i++)
			{
				string line = lines[i];

				string[] fields = csvParser.Split(line);
				for (int f = 0; f < fields.Length; f++)
				{
					fields[f] = fields[f].TrimStart(' ', surround);
					fields[f] = fields[f].TrimEnd(surround);
				}

				if (fields.Length > attributeIndex)
				{
					string key = fields[0];

					if (dictionary.ContainsKey(key)) continue;

					string value = fields[attributeIndex];
					dictionary.Add(key, value);
				}
			}

			return dictionary;
		}

#if UNITY_EDITOR
		public void Add(string key, string value)
		{
			if (filePath == null || filePath.Equals("")) return;

			string append = string.Format("\n\"{0}\",\"{1}\"\"", key, value);
			File.AppendAllText(filePath, append);

			UnityEditor.AssetDatabase.Refresh();
		}

		public void Remove(string key)
		{
			if (filePath == null || filePath.Equals("")) return;

			string[] keys = csvFile.text.Split(lineSeparator);

			for (int i = 0; i < keys.Length; i++)
			{
				if (keys[i].Contains(key))
				{
					string[] newLines = keys.Where(x => x != keys[i]).ToArray();

					string replaced = string.Join(lineSeparator.ToString(), newLines);
					File.WriteAllText(filePath, replaced);

					return;
				}
			}
		}

		public void Edit(string key, string value)
		{
			Remove(key);
			Add(key, value);
		}
#endif
	}
}
