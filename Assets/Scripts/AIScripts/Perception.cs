using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Perception : MonoBehaviour
{
    public string tagName;
	[Range(1, 40)] public float distance = 1;
	[Range(0, 180)] public float angle = 0;
	

	public abstract void GetGameObjects(out List<GameObject> enemies, out List<GameObject> gridObjects);
	//originally returned a list of gameobjects, no outs
}
