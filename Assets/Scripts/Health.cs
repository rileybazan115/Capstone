using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] GameObject deathPrefab;
    [SerializeField] AudioClip deathSound;
    [SerializeField] bool destroyOnDeath;
    [SerializeField] float maxHealth = 100;
    [SerializeField] bool destroyRoot = false;

    public float health { get; set; }
    bool isDead = false;

    void Awake()
	{
        health = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Damage(float damage)
	{
        health -= damage;
        if (!isDead && health <= 0)
		{
            isDead = true;
            /*if (TryGetComponent < IDestructable>(out IDestructable destructable))
			{
                destructable.Destroyed;
			}*/

            if (deathPrefab != null)
			{
                Instantiate(deathPrefab, transform.position, transform.rotation);
                Instantiate(deathSound, transform.position, transform.rotation);
			}

            if (destroyOnDeath)
			{
                if (destroyRoot) Destroy(gameObject.transform.root.gameObject);
                else Destroy(gameObject);
			}
		}
	}
}
