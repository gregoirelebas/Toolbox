using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Toolbox
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextLocaliserUI : MonoBehaviour
	{
		[SerializeField] private LocalisedString localisedString = "";

		private TextMeshProUGUI textField = null;

		private void Awake()
		{
			textField = GetComponent<TextMeshProUGUI>();
			textField.text = LocalisationSystem.GetLocalisedValue(localisedString.key);
		}
	}
}
