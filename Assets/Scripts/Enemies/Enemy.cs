using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int Health;
    public Vector2Int position;

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnEnable()
    {
        GameManager.OnEnemyMove += Act;
    }

    protected virtual void OnDisable()
    {
        GameManager.OnEnemyMove -= Act;
    }

    public abstract void Act();
}
