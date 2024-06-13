using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    public AttackType AttackType;
    public int AttackPower;
    public int Health;
    public GameObject bulletPrefab;
    public TowerCard card;
    protected virtual void OnEnable()
    {
        GameManager.OnTowersAtk += Attack;
    }

    protected virtual void OnDisable()
    {
        GameManager.OnTowersAtk -= Attack;
    }

    public void Start()
    {
        if (card != null)
        {
            AttackType = card.attackType;
            AttackPower = card.attackPower;
            Health = card.health;
        }
    }

    public void Attack()
    {
        Instantiate(bulletPrefab);
    }
}
