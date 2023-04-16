using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
	private Grid<PathNode> grid;
	public int x;
	public int y;
	public int z;

	//how much this node cost
	public float gCost;
	//estimated cost to reach end goal from this node
	public float hCost;
	//g and h combined
	public float fCost;

	public bool isWalkable;
	public PathNode cameFromNode;

    public PathNode(Grid<PathNode> grid, int x, int y, int z)
	{
		this.grid = grid;
		this.x = x;
		this.y = y;
		this.z = z;
		isWalkable = true;
	}

	public void CalculateFCost()
	{
		fCost = gCost + hCost;
	}

	public void SetIsWalkable(bool isWalkable)
	{
		this.isWalkable = isWalkable;
		grid.TriggerGridObjectChanged(x, y, z);
	}

	public override string ToString()
	{
		return x + ", " + y + ", " + z;
	}
}
