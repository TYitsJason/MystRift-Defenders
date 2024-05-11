using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStructure : MonoBehaviour
{
    public int Health {  get; protected set; }

    protected virtual void Awake()
    {

    }

    public virtual void TakeDamage (int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
