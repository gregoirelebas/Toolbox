using System;
using System.Collections;
using System.Collections.Generic;
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

        public void LoadCSVFile(string path)
        {
            csvFile = Resources.Load<TextAsset>(path);

            if (csvFile == null)
            {
                Debug.LogError("Error trying to load " + path + ", file not found!");
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
    }
}
