using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;

public class Fountain : MonoBehaviour
{
	[SerializeField] private GameObject cubePrefab = null;

	private void Awake()
	{
		PoolingHelper.CreatePool("Fountain", cubePrefab, 300, transform);
	}

	private void FixedUpdate()
	{
		PoolingHelper.SpawnFromPool("Fountain");
	}

	private void OnDestroy()
	{
		PoolingHelper.DestroyPool("Fountain");
	}
}
