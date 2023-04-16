using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMark : MonoBehaviour
{
	public string tagName;
	[Range(1, 40)] public float distance = 1;
	[Range(0, 180)] public float angle = 0;

	[SerializeField] Transform raycastTransform;
	[SerializeField][Range(2, 50)] public int numRaycast = 2;
	[SerializeField][Range(0, 5)] public float radius = 2;

	private void Update()
	{
		GetGameObjects(out List<GameObject> gridObjects);

		foreach (GameObject go in gridObjects)
		{

		}
	}

	public void GetGameObjects(out List<GameObject> gridObjects)
	{
		gridObjects = new List<GameObject>();

		float angleOffset = (angle * 2) / (numRaycast - 1);
		for (int i = 0; i < numRaycast; i++)
		{
			Quaternion rotation = Quaternion.AngleAxis(-angle + (angleOffset * i), Vector3.up);
			Vector3 direction = rotation * raycastTransform.forward;
			Ray ray = new Ray(raycastTransform.position, direction);
			distance = (2 / Mathf.Abs(rotation.y));
			if (Physics.SphereCast(ray, radius, out RaycastHit raycastHit, distance))
			{
				if (raycastHit.collider.CompareTag("Cell"))
				{
					Debug.DrawLine(ray.origin, ray.direction * raycastHit.distance, Color.green);
					gridObjects.Add(raycastHit.collider.gameObject);
				}
			}
			else
			{
				Debug.DrawRay(ray.origin, ray.direction * distance, Color.white);
			}
		}
	}
}
