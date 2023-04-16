using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
	[SerializeField] private HeatMapVisual heatMapVisual;
	[SerializeField] private HeatMapBoolVisual heatMapBoolVisual;
	[SerializeField] private HeatMapGenericVisual heatMapGenericVisual;
	private Grid<HeatMapGridObject> grid;

    // Start is called before the first frame update
    private void Start()
    {
        grid = new Grid<HeatMapGridObject>(5, 1, 5, 3f, new Vector3(0, 0, 0), (Grid<HeatMapGridObject> g, int x, int y, int z) => new HeatMapGridObject(g, x, y, z));
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

public class HeatMapGridObject
{
	private const int MIN = 0;
	private const int MAX = 50;

	private Grid<HeatMapGridObject> grid;
	private int x;
	private int y;
	private int z;
	private int value;

	public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y, int z)
	{
		this.grid = grid;
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public void AddValue(int addValue)
	{
		value += addValue;
		value = Mathf.Clamp(value, MIN, MAX);
		grid.TriggerGridObjectChanged(x, y, z);
	}

	public float GetValueNormalized()
	{
		return (float)value / MAX;
	}

	public int GetValue()
	{
		return value;
	}

	public void SetValue(int value)
	{
		this.value = value;
	}

	public override string ToString()
	{
		return value.ToString();
	}
}
