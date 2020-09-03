using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateCity : MonoBehaviour
{
	[SerializeField] private List<Material> materials = null;	
    [SerializeField] private float generationDiameter = 2.0f;
    [SerializeField] private Vector2 buildingHeightFork = Vector2.one;
    [SerializeField] private float buildingWidth = 1.0f;
	[SerializeField] float buildingSpacing = 1.0f;

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
		Gizmos.DrawCube(transform.position + Vector3.up * buildingHeightFork.y / 2.0f, new Vector3(generationDiameter, buildingHeightFork.y, generationDiameter));
	}

	public void Generate()
	{
		Clear();

		int buildingCount = (int)(generationDiameter / (buildingWidth + buildingSpacing));

		float leftSpace = generationDiameter - buildingCount * (buildingWidth + buildingSpacing) + buildingSpacing;

		for (int i = 0; i < buildingCount; i++)
		{
			for (int j = 0; j < buildingCount; j++)
			{
				Transform building = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;

				building.parent = transform;
				building.gameObject.isStatic = true;

				building.localScale = new Vector3(buildingWidth, Random.Range(buildingHeightFork.x, buildingHeightFork.y), buildingWidth);
				building.position = new Vector3(
					transform.position.x + leftSpace / 2.0f - generationDiameter / 2.0f + buildingWidth / 2.0f + i * (buildingWidth + buildingSpacing),
					transform.position.y + building.localScale.y / 2.0f,
					transform.position.z + leftSpace / 2.0f - generationDiameter / 2.0f + buildingWidth / 2.0f + j * (buildingWidth + buildingSpacing));

				building.GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Count)];
			}
		}
	}

	public void Clear()
	{
		int childCount = transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
	}
}
