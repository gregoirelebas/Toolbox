using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox
{
	public class TemporarySingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T instance = null;
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					SetInstance();
				}

				return instance;
			}
		}

		private static void SetInstance(T current = null)
		{
			if (current == null)
			{
				GameObject go = Resources.Load<GameObject>(typeof(T).ToString() + ".prefab");
				instance = go.GetComponent<T>();
			}
			else
			{
				instance = current;
			}
		}

		protected virtual void Awake()
		{
			if (instance == null)
			{
				SetInstance(GetComponent<T>());
			}
			else if (instance != this)
			{
				Destroy(gameObject);
				return;
			}
		}
	}
}
