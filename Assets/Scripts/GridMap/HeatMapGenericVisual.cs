using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapGenericVisual : MonoBehaviour
{
	public GameObject prefab;

    private Grid<HeatMapGridObject> grid;
	private Mesh mesh;
	private float waitTime = 1;
	int index = 0;
	List<GameObject> goGrid = new List<GameObject>();

	private void Awake()
	{
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	public void SetGrid(Grid<HeatMapGridObject> grid)
	{
		this.grid = grid;
		//UpdateHeatMapVisual();
		UpdateHeatMapGO();

		grid.OnGridObjectChanged += Grid_OnGridObjectChanged;
	}

	private void Update()
	{
		waitTime -= Time.deltaTime;
		if (waitTime < 0)
		{
			for (int i = 0; i < grid.GetWidth(); i++)
			{
				for (int j = 0; j < grid.GetHeight(); j++)
				{
					for (int k = 0; k < grid.GetWidth(); k++)
					{
						grid.GetGridObject(i, j, k).SetValue(grid.GetGridObject(i, j, k).GetValue() - 100);
					}	
				}
			}
			waitTime = 1;
		}
	}

	private void Grid_OnGridObjectChanged(object sender, Grid<HeatMapGridObject>.OnGridObjectChangedEventArgs e)
	{
		Debug.Log("Grid_OnGridValueChanged");
		//UpdateHeatMapVisual();
		UpdateHeatMapGO();
	}

	private void UpdateHeatMapGO()
	{
		if (goGrid.Count > 0)
		{
			foreach (var go in goGrid)
			{
				Destroy(go);
			}
			goGrid.Clear();
		}
		
		for (int x = 0; x < grid.GetWidth(); x++)
		{
			for (int y = 0; y < grid.GetHeight(); y++)
			{
				for (int z = 0; z < grid.GetDepth(); z++)
				{
					int index = x * grid.GetHeight() * grid.GetDepth() + y * grid.GetDepth() + z;
					Vector3 quadSize = new Vector3(1, 1, 1) * grid.GetCellSize();

					HeatMapGridObject gridObject = grid.GetGridObject(x, y, z);
					float gridValueNormalized = gridObject.GetValueNormalized();
					Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);

					GameObject go = Instantiate(prefab, grid.GetWorldPosition(x, y, z) + quadSize * 0.5f,  Quaternion.identity, transform);
					go.transform.localScale = Vector3.one * grid.GetCellSize();

					go.GetComponent<Renderer>().material.SetColor("_Color", Color.HSVToRGB(gridValueNormalized, 1, 1));

					goGrid.Add(go);
				}
			}
		}
	}

	private void UpdateHeatMapVisual()
	{
		MeshUtils.CreateEmptyMeshArrays3d(grid.GetWidth() * grid.GetHeight() * grid.GetDepth(), out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);

		for (int x = 0; x < grid.GetWidth(); x++)
		{
			for (int y = 0; y < grid.GetHeight(); y++)
			{
				for (int z = 0; z < grid.GetDepth(); z++)
				{
					int index = x * grid.GetHeight() * grid.GetDepth() + y * grid.GetDepth() + z;
					Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

					HeatMapGridObject gridObject = grid.GetGridObject(x, y, z);
					float gridValueNormalized = gridObject.GetValueNormalized();
					Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);
					MeshUtils.AddToMeshArrays3d(vertices, uvs, triangles, index, grid.GetWorldPosition(x, y, z) + quadSize * 0.5f, 0f, quadSize, gridValueUV, gridValueUV);
					Debug.Log(index);
				}
			}
		}

		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
	}
}
