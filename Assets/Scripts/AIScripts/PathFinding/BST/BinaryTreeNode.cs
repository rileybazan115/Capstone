using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BinaryTreeNode<T> where T : IComparable<T>
{
	public BinaryTreeNode<T> Left { get; set; }
	public BinaryTreeNode<T> Right { get; set; }
	public T Data { get; set; }

	public BinaryTreeNode(T val)
	{
		this.Data = val;
	}

	public void Add(T val)
	{
		if (Data.CompareTo(val) > 0)
		{
			if (Left != null)
			{
				Left.Add(val);
			}
			else
			{
				Left = new BinaryTreeNode<T>(val);
			}
		}
		if (Data.CompareTo(val) < 0)
		{
			if (Right != null)
			{
				Right.Add(val);
			}
			else
			{
				Right = new BinaryTreeNode<T>(val);
			}
		}
	}

	public bool Contains(T val)
	{
		bool contains = false;

		if (Data.CompareTo(val) < 0 && Right != null)
		{
			contains = Right.Contains(val);
		}
		if (Data.CompareTo(val) > 0 && Left != null)
		{
			contains  =Left.Contains(val);
		}
		if (Data.Equals(val))
		{
			contains = true;
		}

		return contains;
	}

	public BinaryTreeNode<T> Remove(T val)
	{
		if (val.CompareTo(Data) < 0)
		{
			Left = Left.Remove(val);
		}
		if (val.CompareTo(Data) > 0)
		{
			Right = Right.Remove(val);
		}
		if (val.CompareTo(Data) == 0)
		{
			if (Left == null)
			{
				return Right;
			}
			else if (Right == null)
			{
				return Left;
			}

			Data = FindBiggestLeft(Left);
			Left = Left.Remove(Data);
		}
		return this;
	}

	public T FindBiggestLeft(BinaryTreeNode<T> node)
	{
		while (node.Right != null)
		{
			node = node.Right;
		}

		return node.Data;
	}
}
