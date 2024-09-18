using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyStructure : MonoBehaviour
{
    public int Health;

    protected virtual void Awake()
    {

    }

    protected virtual void OnEnable()
    {
        GameManager.OnStructure += OnAct;
    }
    protected virtual void OnDisable()
    {
        GameManager.OnStructure -= OnAct;
    }

    protected abstract void OnAct();

    public virtual void TakeDamage (int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
