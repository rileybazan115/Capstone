using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherecastPerception : Perception
{
    [SerializeField] float waitTime = 0;
    [SerializeField] float detectionFloat = 0;
    [SerializeField] Transform raycastTransform;
    [SerializeField] [Range(2, 50)] public int numRaycast = 2;
    [SerializeField] [Range(0, 5)] public float radius = 2;
	int x, y, z;
    public override void GetGameObjects(out List<GameObject> enemies, out List<GameObject> gridObjects)
    {
		enemies = new List<GameObject>();
		gridObjects = new List<GameObject>();
		
		float temp = waitTime;

		float angleOffset = (angle * 2) / (numRaycast - 1);
		for (int i = 0; i < numRaycast; i++)
		{
			Quaternion rotation = Quaternion.AngleAxis(-angle + (angleOffset * i), Vector3.up);
			Vector3 direction = rotation * raycastTransform.forward;
			Ray ray = new Ray(raycastTransform.position, direction);
			distance = (2 / Mathf.Abs(rotation.y));
			if (Physics.SphereCast(ray, radius, out RaycastHit raycastHit, distance))
			{
				if (tagName == "" || raycastHit.collider.CompareTag(tagName))
				{
					Debug.DrawRay(ray.origin, ray.direction * raycastHit.distance, Color.red);
					//Debug.Log("enemy seen");
					detectionFloat += 1;
					enemies.Add(raycastHit.collider.gameObject);
					/*if (detectionFloat == 100)
					{
						Debug.Log("chasing");
						enemies.Add(raycastHit.collider.gameObject);
					}*/
				}

				if (raycastHit.collider.CompareTag("Grid"))
				{
					Debug.DrawLine(ray.origin, ray.direction * raycastHit.distance, Color.green);
					gridObjects.Add(raycastHit.collider.gameObject);
					Debug.Log("chasing");
				}
			}
			else
			{
				Debug.DrawRay(ray.origin, ray.direction * distance, Color.white);
				detectionFloat -= 1;
			}
		}
		waitTime = temp;
	}
}
