using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FountainCube : MonoBehaviour
{
	[SerializeField] private float spawnRadius = 5.0f;
	[SerializeField] private Vector3 spawnForce = Vector3.one;

	private Rigidbody rb = null;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void OnEnable()
	{
		transform.localPosition = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0.0f, Random.Range(-spawnRadius, spawnRadius));
		rb.AddForce(new Vector3(Random.Range(-spawnForce.x, spawnForce.x), spawnForce.y, Random.Range(-spawnForce.z, spawnForce.z)));
	}

	private void OnDisable()
	{
		rb.velocity = Vector3.zero;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}
}
