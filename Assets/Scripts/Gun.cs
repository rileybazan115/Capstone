using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	[SerializeField] float fireRate;
	[SerializeField] bool automatic;
	[SerializeField] GameObject ammoPrefab;
	[SerializeField] Transform projectileSpawn;
	private float timer;
	
	public void Fire()
	{
		if (Time.time > timer)
		{
			timer = Time.time + fireRate;
			Instantiate(ammoPrefab, projectileSpawn.position, projectileSpawn.rotation);
		}
	}
}
