using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
	public const int HEAT_MAP_MAX_VALUE = 100;
	public const int HEAT_MAP_MIN_VALUE = 0;

	public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
	public class OnGridObjectChangedEventArgs : EventArgs
	{
		public int x;
		public int y;
		public int z;
	}

	private int width;
	private int height;
	private int depth;
	private float cellSize;
	private Vector3 originPosition;
	private TGridObject[,,] gridArray;
	private TextMesh[,,] debugTextArray;

	public Grid (int width, int height, int depth, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, int, TGridObject> createGridObject)
	{
		this.width = width;
		this.height = height;
		this.depth = depth;
		this.cellSize = cellSize;
		this.originPosition = originPosition;

		gridArray = new TGridObject[width, height, depth];
		debugTextArray = new TextMesh[width, height, depth];

		for (int x = 0; x < gridArray.GetLength(0); x++)
		{
			for (int y = 0; y < gridArray.GetLength(1); y++)
			{
				for (int z = 0; z < gridArray.GetLength(2); z++)
				{
					gridArray[x, y, z] = createGridObject(this, x, y, z);
				}
			}
		}

		//bool debug
		for (int x = 0; x < gridArray.GetLength(0); x++)
		{
			for (int y = 0; y < gridArray.GetLength(1); y++)
			{
				for (int z = 0; z < gridArray.GetLength(2); z++)
				{
					debugTextArray[x, y, z] = Utils.CreateWorldText(gridArray[x, y, z]?.ToString(), null, GetWorldPosition(x, y, z) + new Vector3(cellSize, cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);
					//debugTextArray[x, y, z].transform.rotation = Quaternion.AngleAxis(90, Vector3.right);
					/*Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y + 1, z), Color.white, 100f);
					Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x + 1, y, z), Color.white, 100f);
					Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z + 1), Color.white, 100f);*/

					Debug.DrawLine(GetWorldPosition(x, y + 1, z), GetWorldPosition(x, y + 1, z + 1), Color.white, 100f);
					Debug.DrawLine(GetWorldPosition(x, y + 1, z), GetWorldPosition(x + 1, y + 1, z), Color.white, 100f);
				}
			}
		}
		
		//Debug.DrawLine(GetWorldPosition(width, 0, 0), GetWorldPosition(width, height, 0), Color.white, 100f);
		//Debug.DrawLine(GetWorldPosition(width, 0, 0), GetWorldPosition(width, 0, depth), Color.white, 100f);
		Debug.DrawLine(GetWorldPosition(width, height, 0), GetWorldPosition(width, height, depth), Color.white, 100f);
		//Debug.DrawLine(GetWorldPosition(width, 0, depth), GetWorldPosition(width, height, depth), Color.white, 100f);
		//Debug.DrawLine(GetWorldPosition(0, height, 0), GetWorldPosition(0, height, depth), Color.white, 100f);
		Debug.DrawLine(GetWorldPosition(width, height, depth), GetWorldPosition(0, height, depth), Color.white, 100f);
		//Debug.DrawLine(GetWorldPosition(width, 0, depth), GetWorldPosition(0, 0, depth), Color.white, 100f);
		//Debug.DrawLine(GetWorldPosition(0, 0, depth), GetWorldPosition(0, height, depth), Color.white, 100f);

		OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
		{
			debugTextArray[eventArgs.x, eventArgs.y, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.y, eventArgs.z].ToString();
		};
	}

	public Vector3 GetWorldPosition(int x, int y, int z)
	{
		return new Vector3(x, y, z) * cellSize + originPosition;
	}

	public void GetXYZ(Vector3 worldPosition, out int x, out int y, out int z)
	{
		x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
		y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
		z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
	}

	public void SetGridObject(int x, int y, int z, TGridObject value)
	{
		if (x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < depth)
		{
			//gridArray[x, y, z] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
			gridArray[x, y, z] = value;
			if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y, z = z });
			debugTextArray[x, y, z].text = gridArray[x, y, z].ToString();
		}
	}

	public void TriggerGridObjectChanged(int x, int y, int z)
	{
		if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y, z = z}); 
	}

	public void SetGridObject(Vector3 worldPosition, TGridObject value) 
	{
		int x, y, z;
		GetXYZ(worldPosition, out x, out y, out z);
		SetGridObject(x, y, z, value);
	}

	public TGridObject GetGridObject(int x, int y, int z)
	{
		if (x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < depth)
		{
			return gridArray[x, y, z];
		}
		else
		{
			return default(TGridObject);
		}
	}

	public TGridObject GetGridObject(Vector3 worldPostion)
	{
		int x, y, z;
		GetXYZ(worldPostion, out x, out y, out z);
		return GetGridObject(x, y, z);
	}

	public int GetWidth()
	{
		return gridArray.GetLength(0);
	}

	public int GetHeight()
	{
		return gridArray.GetLength(1);
	}

	public int GetDepth()
	{
		return gridArray.GetLength(2);
	}

	public float GetCellSize()
	{
		return cellSize;
	}
}
