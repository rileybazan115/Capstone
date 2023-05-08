using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapGridObject : MonoBehaviour
{
	private const int MIN = 0;
	private const int MAX = 100;

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
