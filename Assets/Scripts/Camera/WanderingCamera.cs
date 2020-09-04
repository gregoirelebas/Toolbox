using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;

public class WanderingCamera : MonoBehaviour
{
	[SerializeField] private float wanderingRadius = 5.0f;
	[SerializeField] private float lerpSpeed = 1.0f;

	private Vector3 initialPosition = Vector3.zero;
	private Vector3 newPosition = Vector3.zero;

	private void Awake()
	{
		initialPosition = transform.position;
		newPosition = FindNewPosition();
	}

	private void Update()
	{
		transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * lerpSpeed);
		
		if (Vector3.Distance(transform.position, newPosition) < 2.0f)
		{
			newPosition = FindNewPosition();
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1.0f, 0.0f, 1.0f, 0.25f);
		Gizmos.DrawSphere(transform.position, wanderingRadius);
	}

	private Vector3 FindNewPosition()
	{
		float x = Random.Range(initialPosition.x - wanderingRadius, initialPosition.x + wanderingRadius);
		float z = Random.Range(initialPosition.z - wanderingRadius, initialPosition.z + wanderingRadius);

		return new Vector3(x, initialPosition.y, z);
	}
}
