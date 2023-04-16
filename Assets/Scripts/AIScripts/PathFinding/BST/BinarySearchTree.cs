using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySearchTree<T> where T : IComparable<T>
{
    public BinaryTreeNode<T> Root { get; set; }

    public int Count { get; set; } = 0;

    public void Add(T val)
	{
        if (Root == null)
		{
			Root = new BinaryTreeNode<T>(val);
			Count++;
		}
		else
		{
			Root.Add(val);
			Count++;
		}
	}

	public bool Contains(T val)
	{
		return Root.Contains(val);
	}

	public void Remove(T val)
	{
		if (Contains(val) == true)
		{
			Root = Root.Remove(val);
			Count--;
		}
		else
		{
			Root = Root.Remove(val);
			Count--;
		}
	}

	public void Clear()
	{
		Root = null;
		Count = 0;
	}
}
