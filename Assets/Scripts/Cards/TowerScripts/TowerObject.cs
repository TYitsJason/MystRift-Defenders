using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerObject : MonoBehaviour
{
    public int Power;
    public int bonusPower = 0;
    public int Health;
    public GridCell Position;
    public CardInstance card;
    public bool manual = false;
    protected virtual void OnEnable()
    {
        GameManager.OnTowersAtk += Attack;
    }

    protected virtual void OnDisable()
    {
        GameManager.OnTowersAtk -= Attack;
    }

    public abstract void Attack();

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            card.isPlaced = false;
            Position.DestroyPlacedTower();
        }
        TowerCard towerCard = card.cardData as TowerCard;
        if (Health > towerCard.health)
        {
            Health = towerCard.health;
        }
    } 
}
