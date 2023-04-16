using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] ForceMode forceMode;
    [SerializeField] float timer;
    [SerializeField] GameObject destroyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb.AddRelativeForce(Vector3.forward * speed, forceMode);
        if (timer != 0) StartCoroutine(DestroyTime());
    }

	public void Destroy()
	{
        if (destroyPrefab != null) Instantiate(destroyPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

	IEnumerator DestroyTime()
	{
        yield return new WaitForSeconds(timer);

        Destroy();
	}

	private void OnCollisionEnter(Collision collision)
	{
        if (timer != 0) return;

        Destroy();
	}
}
