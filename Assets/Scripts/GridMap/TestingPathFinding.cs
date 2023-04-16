using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPathFinding : MonoBehaviour
{
	[SerializeField] private PathfindingVisual pathfindingVisual;
	[SerializeField] private NavMeshMovement navMeshMovement;
	[SerializeField] private GameObject agent;
	[SerializeField] private int x;
	[SerializeField] private int y;
	[SerializeField] private int z;
	private Pathfinding pathfinding;

	private void Start()
	{
		pathfinding = new Pathfinding(x, y, z);
		pathfindingVisual.SetGrid(pathfinding.GetGrid());
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition3D();
			pathfinding.GetGrid().GetXYZ(mouseWorldPosition, out int x, out int y, out int z);
			List<PathNode> path = pathfinding.FindPath(0, 0, 0, x, y, z);
			//Vector3 temp = new Vector3(x, y, z);
			//List<PathNode> path = pathfinding.FindPath((int)agent.transform.position.x, (int)agent.transform.position.y, (int)agent.transform.position.z, x, y, z);
			if (path != null)
			{
				for (int i = 0; i < path.Count - 1; i++)
				{
					Debug.DrawLine(new Vector3(path[i].x, path[i].y, path[i].z) * 5f + Vector3.one * 5f, 
						new Vector3(path[i + 1].x, path[i + 1].y, path[i + 1].z) * 5f + Vector3.one * 5f, 
						Color.green, 10);
				}
				
			}
			navMeshMovement.SetTargetPosition(mouseWorldPosition);
		}

		if (Input.GetMouseButtonDown(1))
		{
			Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition3D();
			pathfinding.GetGrid().GetXYZ(mouseWorldPosition, out int x, out int y, out int z);
			pathfinding.GetNode(x, y, z).SetIsWalkable(!pathfinding.GetNode(x, y, z).isWalkable);
		}
	}
}
