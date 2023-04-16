using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition
{
	Condition[] conditions;
	bool active;

	public Transition(Condition[] conditions, bool active)
	{
		this.conditions = conditions;
		this.active = active;
	}

	public bool ToTransition()
	{
		foreach (var condition in conditions)
		{
			if (!condition.IsTrue() || !active) return false;
		}

		return true;
	}
}
