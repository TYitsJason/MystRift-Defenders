using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime;
    public float maxTime;

    public float travelSpeed;

    public int damage;
    public Rigidbody rb;

    private void Start()
    {
        lifeTime = 0;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > maxTime)
        {
            Destroy(gameObject);
            return;
        }
        rb.MovePosition(transform.position + Vector3.right * travelSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Structure"))
        {
            collision.gameObject.GetComponent<EnemyStructure>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
