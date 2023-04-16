using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingVisual : MonoBehaviour
{
    private Grid<PathNode> grid;
    private Mesh mesh;
    private bool updateMesh;

	private void Awake()
	{
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	public void SetGrid(Grid<PathNode> grid)
	{
		this.grid = grid;
		UpdateVisual();

		grid.OnGridObjectChanged += Grid_OnGridValueChanged;
	}

	private void Grid_OnGridValueChanged(object sender, Grid<PathNode>.OnGridObjectChangedEventArgs e)
	{
		updateMesh = true;
	}

	private void LateUpdate()
	{
		if (updateMesh)
		{
			updateMesh = false;
			UpdateVisual();
		}
	}

	private void UpdateVisual()
	{
		MeshUtils.CreateEmptyMeshArrays3d(grid.GetWidth() * grid.GetHeight() * grid.GetDepth(), 
			out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);

		for (int x = 0; x < grid.GetWidth(); x++)
		{
			for (int y = 0; y < grid.GetHeight(); y++)
			{
				for (int z = 0; z < grid.GetDepth(); z++)
				{
					int index = x * grid.GetHeight() * grid.GetDepth() + y * grid.GetDepth() + z;
					//Vector3 quadSize = Quaternion.AngleAxis(90, Vector3.down) * new Vector3(1, 1, 1) * grid.GetCellSize();
					Vector3 quadSize = new Vector3(1, 1, 1) * grid.GetCellSize();

					PathNode pathNode = grid.GetGridObject(x, y, z);

					if (pathNode.isWalkable)
					{
						quadSize = Vector3.zero;
					}

					MeshUtils.AddToMeshArrays3d(vertices, uvs, triangles, index, grid.GetWorldPosition(x, y, z) + quadSize * 0.5f, 0f, quadSize, 
						Vector2.zero, Vector2.zero);
				}
			}
		}

		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
	}
}
