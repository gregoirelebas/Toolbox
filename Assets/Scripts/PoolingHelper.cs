using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox
{
	public static class PoolingHelper
	{
		[System.Serializable]
		private class Pool
		{
			public string tag = "";
			public GameObject prefab = null;
			public Queue<GameObject> stock = new Queue<GameObject>();
			public int size = 0;

			public Pool(string tag, GameObject prefab, int size)
			{
				this.tag = tag;
				this.prefab = prefab;
				this.size = size;
			}
		}

		private static Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

		/// <summary>
		/// Create and fill and new pool of GameObject.
		/// </summary>
		public static void CreatePool(string tag, GameObject prefab, int size, Transform parent = null)
		{
			if (pools.ContainsKey(tag))
			{
				Debug.LogWarning("Pool with tag " + tag + " already exist.");
				return;
			}

			Pool newPool = new Pool(tag, prefab, size);

			for (int i = 0; i < size; i++)
			{
				GameObject go = Object.Instantiate(prefab, parent);
				go.SetActive(false);
				newPool.stock.Enqueue(go);
			}

			pools.Add(tag, newPool);
		}

		/// <summary>
		/// Destroy all GameObject in pool and remove it.
		/// </summary>
		public static void DestroyPool(string tag)
		{
			if (!pools.ContainsKey(tag))
			{
				Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
				return;
			}

			foreach (GameObject go in pools[tag].stock)
			{
				Object.Destroy(go);
			}

			pools[tag].stock.Clear();
			pools.Remove(tag);
		}

		/// <summary>
		/// Spawn (activate) a new GameObject from pool and return it. 
		/// </summary>
		public static GameObject SpawnFromPool(string tag)
		{
			if (!pools.ContainsKey(tag))
			{
				Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
				return null;
			}

			GameObject toSpawn = pools[tag].stock.Dequeue();
			toSpawn.SetActive(true);
			pools[tag].stock.Enqueue(toSpawn);

			return toSpawn;
		}
	}
}