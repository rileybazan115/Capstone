using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
	/*private const int MOVE_STRAIGHT_COST = 10;
	private const int MOVE_DIAGONAL_COST = 14;*///Mathf.Sqrt(2) * 10;
	private const float MOVE_STRAIGHT_COST = 10;
	private const float MOVE_DIAGONAL_COST = 14.121f;

	public static Pathfinding Instance { get; private set; }

	private Grid<PathNode> grid;
	private List<PathNode> openList;
	private List<PathNode> closedList;

    public Pathfinding(int width, int height, int depth)
	{
		Instance = this;
		grid = new Grid<PathNode>(width, height, depth, 5f, new Vector3(0, 0, 0), (Grid<PathNode> grid, int x, int y, int z) => new PathNode(grid, x, y, z));
	}

	public Grid<PathNode> GetGrid()
	{
		return grid;
	}

	public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
	{
		grid.GetXYZ(startWorldPosition, out int startX, out int startY, out int startZ);
		grid.GetXYZ(endWorldPosition, out int endX, out int endY, out int endZ);

		List<PathNode> path = FindPath(startX, startY, startZ, endX, endY, endZ);
		if (path == null)
		{
			return null;
		}
		else
		{
			List<Vector3> vectorPath = new List<Vector3>();
			foreach (PathNode pathNode in path)
			{
				Debug.Log("PATH");
				vectorPath.Add(new Vector3(pathNode.x, pathNode.y, pathNode.z) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
			}
			return vectorPath;
		}
	}

	public List<PathNode> FindPath(int startX, int startY, int startZ, int endX, int endY, int endZ)
	{
		PathNode startNode = grid.GetGridObject(startX, startY, startZ);
		PathNode endNode = grid.GetGridObject(endX, endY, endZ);

		/*Debug.Log(startX + " " + startY + " " + startZ);
		Debug.Log(endX + " " + endY + " " + endZ);*/

		//ckecks if points are valid
		if (startNode == null || endNode == null)
		{
			return null;
		}

		openList = new List<PathNode>() { startNode };
		closedList = new List<PathNode>();

		//resetting grid
		for (int x = 0; x < grid.GetWidth(); x++)
		{
			for (int y = 0; y < grid.GetHeight(); y++)
			{
				for (int z = 0; z < grid.GetDepth(); z++)
				{
					PathNode pathNode = grid.GetGridObject(x, y, z);
					pathNode.gCost = int.MaxValue;
					pathNode.CalculateFCost();
					pathNode.cameFromNode = null;
				}
			}
		}

		startNode.gCost = 0;
		startNode.hCost = CalculateDistanceCost(startNode, endNode);
		startNode.CalculateFCost();

		while(openList.Count > 0)
		{
			//if lowest fcost is the endnode, a path is found, and calculated
			PathNode currentNode = GetLowestFCostNode(openList);
			if (currentNode == endNode)
			{
				return CalculatePath(endNode);
			}

			openList.Remove(currentNode);
			closedList.Add(currentNode);

			foreach (PathNode neighbourNode in GetNeighborList(currentNode))
			{
				//if the neighbor is alread in the closed list, or isnt walkable, try next node
				if (closedList.Contains(neighbourNode)) continue;
				if (!neighbourNode.isWalkable)
				{
					closedList.Add(neighbourNode);
					continue;
				}

				float tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
				if (tentativeGCost < neighbourNode.gCost)
				{
					neighbourNode.cameFromNode = currentNode;
					neighbourNode.gCost = tentativeGCost;
					neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
					neighbourNode.CalculateFCost();

					if (!openList.Contains(neighbourNode))
					{
						openList.Add(neighbourNode);
					}
				}
			}
		}
		//out of nodes on open list
		return null;
	}

	private List<PathNode> GetNeighborList(PathNode currentNode)
	{
		List<PathNode> neighborList = new List<PathNode>();

		/*for (int z = -1; z < 2; z++)
		{
			for (int y = -1; y < 2; y++)
			{
				for (int x = -1; x < 2; x++)
				{
					if (currentNode.x + x >= 0 && currentNode.y + y >= 0 && currentNode.z + z >= 0)
					{
						neighborList.Add(GetNode(currentNode.x + x, currentNode.y + y, currentNode.z + z));
					}
				}
			}
		}*/

		//left		
		if (currentNode.x - 1 >= 0)
		{
			//left
			neighborList.Add(GetNode(currentNode.x - 1, currentNode.y, currentNode.z));
			//left up
			if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y + 1, currentNode.z));
			//left down
			if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y - 1, currentNode.z));
		}
		//right
		if (currentNode.x + 1 < grid.GetWidth())
		{
			//right
			neighborList.Add(GetNode(currentNode.x + 1, currentNode.y, currentNode.z));
			//right up
			if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y + 1, currentNode.z));
			//right down
			if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y - 1, currentNode.z));
		}
		//up
		if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x, currentNode.y + 1, currentNode.z));
		//down
		if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x, currentNode.y - 1, currentNode.z));

		//above
		if (currentNode.z + 1 < grid.GetDepth())
		{
			neighborList.Add(GetNode(currentNode.x, currentNode.y, currentNode.z + 1));

			if (currentNode.x - 1 >= 0)
			{
				//left
				neighborList.Add(GetNode(currentNode.x - 1, currentNode.y, currentNode.z + 1));
				//left up
				if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y + 1, currentNode.z + 1));
				//left down
				if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y - 1, currentNode.z + 1));
			}
			//right
			if (currentNode.x + 1 < grid.GetWidth())
			{
				//right
				neighborList.Add(GetNode(currentNode.x + 1, currentNode.y, currentNode.z + 1));
				//right up
				if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y + 1, currentNode.z + 1));
				//right down
				if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y - 1, currentNode.z + 1));
			}
			//up
			if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x, currentNode.y + 1, currentNode.z + 1));
			//down
			if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x, currentNode.y - 1, currentNode.z + 1));
		}

		//below
		if (currentNode.z - 1 >= 0)
		{
			neighborList.Add(GetNode(currentNode.x, currentNode.y, currentNode.z - 1));
			if (currentNode.x - 1 >= 0)
			{
				//left
				neighborList.Add(GetNode(currentNode.x - 1, currentNode.y, currentNode.z - 1));
				//left up
				if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y + 1, currentNode.z - 1));
				//left down
				if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y - 1, currentNode.z - 1));
			}
			//right
			if (currentNode.x + 1 < grid.GetWidth())
			{
				//right
				neighborList.Add(GetNode(currentNode.x + 1, currentNode.y, currentNode.z - 1));
				//right up
				if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y + 1, currentNode.z - 1));
				//right down
				if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y - 1, currentNode.z - 1));
			}
			//up
			if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x, currentNode.y + 1, currentNode.z - 1));
			//down
			if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x, currentNode.y - 1, currentNode.z - 1));
		}

		return neighborList;
	}

	public PathNode GetNode(int x, int y, int z)
	{
		return grid.GetGridObject(x, y, z);
	}

	private List<PathNode> CalculatePath(PathNode endNode)
	{
		List<PathNode> path = new List<PathNode>();
		path.Add(endNode);
		PathNode currentNode = endNode;
		while (currentNode.cameFromNode != null)
		{
			path.Add(currentNode.cameFromNode);
			currentNode = currentNode.cameFromNode;
		}
		path.Reverse();
		return path;
	}

	private float CalculateDistanceCost(PathNode a, PathNode b)
	{
		int xDistance = Mathf.Abs(a.x - b.x);
		int yDistance = Mathf.Abs(a.y - b.y);
		int zDistance = Mathf.Abs(a.z - b.z);
		int remaining = Mathf.Abs(xDistance - yDistance - zDistance);
		return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
	}

	private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
	{
		PathNode lowestFCostNode = pathNodeList[0];
		for (int i = 1; i < pathNodeList.Count; i++)
		{
			if (pathNodeList[i].fCost < lowestFCostNode.fCost)
			{
				lowestFCostNode = pathNodeList[i];
			}
		}
		return lowestFCostNode;
	}
}
