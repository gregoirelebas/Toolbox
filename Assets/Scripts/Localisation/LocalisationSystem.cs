using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox
{
	public class LocalisationSystem
	{
		public enum Language
		{
			English,
			French
		}

		public static Language CurrentLanguage { get; private set; } = Language.English;
		public static CSVLoader loader = null;

		private static Dictionary<string, string> localisedEN = null;
		private static Dictionary<string, string> localisedFR = null;

		private static bool isInit = false;

		public static void Init()
		{
			loader = new CSVLoader();
			loader.LoadCSVFile("localisation");

			UpdateDictionaries();			

			isInit = true;
		}

		public static void UpdateDictionaries()
		{
			localisedFR = loader.GetDictionaryValues("fr");
			localisedEN = loader.GetDictionaryValues("en");
		}

		public static void SetLanguage(Language newLanguage)
		{
			CurrentLanguage = newLanguage;
		}

		public static Dictionary<string, string> GetDictionaryForEditor(Language lang)
		{
			if (!isInit) Init();

			switch (lang)
			{
				case Language.English:
					return localisedEN;

				case Language.French:
					return localisedFR;

				default:
					Debug.LogError("Unknown language : " + lang.ToString() + ", return null.");
					return null;
			}
		}

		public static string GetLocalisedValue(string key)
		{
			if (!isInit)
			{
				Init();
			}

			string value = "";

			switch (CurrentLanguage)
			{
				case Language.English:
					localisedEN.TryGetValue(key, out value);
					break;

				case Language.French:
					localisedFR.TryGetValue(key, out value);
					break;

				default:
					Debug.LogError("Unknown language : " + CurrentLanguage + ", return empty string.");
					break;
			}

			return value;
		}

#if UNITY_EDITOR
		public static void Add(string key, string value)
		{
			if (value.Contains("\""))
			{
				//Avoid bugs if value contains any " characters.
				value.Replace('"', '\"');
			}

			if (loader == null)
			{
				loader = new CSVLoader();
			}

			loader.LoadCSVFile("localisation");
			loader.Add(key, value);

			UpdateDictionaries();
		}

		public static void Remove(string key)
		{
			if (loader == null)
			{
				loader = new CSVLoader();
			}

			loader.LoadCSVFile("localisation");
			loader.Remove(key);

			UpdateDictionaries();
		}

		public static void Edit(string key, string value)
		{
			if (value.Contains("\""))
			{
				//Avoid bugs if value contains any " characters.
				value.Replace('"', '\"');
			}

			if (loader == null)
			{
				loader = new CSVLoader();
			}

			loader.LoadCSVFile("localisation");
			loader.Edit(key, value);

			UpdateDictionaries();
		}
#endif
	}
}
