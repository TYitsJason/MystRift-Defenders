using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sell Tower Action", menuName = "Card Effects/Sell Tower")]
public class SellCardAction : CardEffect
{
    public int damage;
    public override void Execute(GridCell cell)
    {
        cell.towerObject.GetComponent<TowerObject>().TakeDamage(damage);
    }
}
