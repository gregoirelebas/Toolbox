using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantSingleton<T> : MonoBehaviour where T : MonoBehaviour
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

			DontDestroyOnLoad(go);
		}
		else
		{
			instance = current;

			DontDestroyOnLoad(current.gameObject);
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
