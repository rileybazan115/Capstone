using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{
	//how to get the grid in here
	
	int lowestValue;
	Vector3 lowestValuePosition;
	Grid<HeatMapGridObject> grid;

	public SeekState(StateAgent owner, string name) : base(owner, name)
	{

	}

	public override void OnEnter()
	{
		//SearchGrid();
		owner.movement.Resume();
		owner.timer.value = Random.Range(5, 10);
		
	}

	public override void OnExit()
	{
		
	}

	public override void OnUpdate()
	{
		//maybe have if statement to get to spawnnode
		//maybe put get value in update, but could be very expensive
		SearchGrid();
	}

	public void SearchGrid()
	{
		//not crashing, but isnt moving
		grid = owner.grid.getGrid();

		//grid.GetLowestValue(out lowestValue, out lowestValuePosition);
		GetLowestValue(out lowestValue, out lowestValuePosition);
		owner.movement.MoveTowards(lowestValuePosition);

		Debug.Log(lowestValue + " " + lowestValuePosition);
	}

	public void GetLowestValue(out int lowestValue, out Vector3 lowestValuePosition)
	{
		lowestValue = int.MaxValue;
		lowestValuePosition = Vector3.zero;

		for (int x = 0; x < grid.GetWidth(); x++)
		{
			for (int y = 0; y < grid.GetHeight(); y++)
			{
				for (int z = 0; z < grid.GetDepth(); z++)
				{
					if (grid.GetGridObject(x, y, z).GetValue() < lowestValue)
					{
						grid.GetGridObject(x, y, z);

						lowestValue = grid.GetGridObject(x, y, z).GetValue();
						lowestValuePosition = grid.GetWorldPosition(x, y, z);
					}
				}
			}
		}
	}

	public void GetNearestLowestValue(Vector3 position, out int lowestValue, out Vector3 lowestValuePosition)
	{
		int x, y, z;
		lowestValue = int.MaxValue;
		lowestValuePosition = Vector3.zero;

		grid.GetXYZ(position, out x, out y, out z);

		if (grid.GetGridObject(x, y, z).GetValue() < lowestValue)
		{
			lowestValue = grid.GetGridObject(x, y, z).GetValue();
		}

		CenterSearch(x, y, z, lowestValue, lowestValuePosition);
	}

	//might need to do something with positive and negatives, not sure
	public void CenterSearch(int x, int y, int z, int lowestValue, Vector3 lowestValuePosition)
	{
		if (grid.GetGridObject(x, y, z).GetValue() < lowestValue)
		{
			lowestValue = grid.GetGridObject(x, y, z).GetValue();
			lowestValuePosition = grid.GetWorldPosition(x, y, z);
		}
		if (x + 1 <= grid.GetWidth())
		{
			CenterSearch(x + 1, y, z, lowestValue, lowestValuePosition);
		}
		else if (y + 1 <= grid.GetHeight())
		{
			CenterSearch(x, y + 1, z, lowestValue, lowestValuePosition);
		}
		/*else if (z + 1 <= grid.GetDepth())
		{
			CenterSearch(x, y, z + 1, lowestValue, lowestValuePosition);
		}*/
	}
}
