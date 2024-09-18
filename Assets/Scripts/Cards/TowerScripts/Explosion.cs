using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float lifeTime;
    private float maxTime;

    public int damage;

    private void Start()
    {
        lifeTime = 0f;
        maxTime = 0.5f;
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > maxTime)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
