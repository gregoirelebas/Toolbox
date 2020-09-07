using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Toolbox
{
	[ExecuteInEditMode]
	public class SetParentNameAsText : MonoBehaviour
	{
		[SerializeField] private bool useTMP = true;

		private void Awake()
		{
			if (useTMP)
			{
				GetComponent<TextMeshProUGUI>().text = transform.parent.name;
			}
			else
			{
				GetComponent<Text>().text = transform.parent.name;
			}
		}

#if UNITY_EDITOR
		private void Update()
		{
			Awake();
		}
#endif
	}
}
