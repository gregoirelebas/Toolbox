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

		private static Dictionary<string, string> localisedEN = null;
		private static Dictionary<string, string> localisedFR = null;

		private static bool isInit = false;

		public static void Init()
		{
			CSVLoader loader = new CSVLoader();
			loader.LoadCSVFile("localisation");

			isInit = true;

			localisedEN = loader.GetDictionaryValues("en");
			localisedFR = loader.GetDictionaryValues("fr");
		}

		public static void SetLanguage(Language newLanguage)
		{
			CurrentLanguage = newLanguage;
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
	}
}
