using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
	[SerializeField] private HeatMapVisual heatMapVisual;
	[SerializeField] private HeatMapBoolVisual heatMapBoolVisual;
	[SerializeField] private HeatMapGenericVisual heatMapGenericVisual;
	[SerializeField] public Transform gridStart;
	private Grid<HeatMapGridObject> grid;

    // Start is called before the first frame update
    private void Start()
    {
        grid = new Grid<HeatMapGridObject>(2, 1, 2, 3f, new Vector3(transform.position.x, transform.position.y, transform.position.z), (Grid<HeatMapGridObject> g, int x, int y, int z) => new HeatMapGridObject(g, x, y, z));
		heatMapGenericVisual.SetGrid(grid);
    }

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 position = Utils.GetMouseWorldPosition();
			HeatMapGridObject heatMapGridObject = grid.GetGridObject(position);
			if (heatMapGridObject != null)
			{
				Debug.Log("click");
				heatMapGridObject.AddValue(5);
			}
		}
	}

	public Grid<HeatMapGridObject> getGrid()
	{
		return grid;
	}
}
