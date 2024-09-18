using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Card Effects/Heal Effect")]
public class HealTower : CardEffect
{
    public int healAmount;
    public override void Execute(GridCell cell)
    {
        cell.towerObject.GetComponent<TowerObject>().TakeDamage(-healAmount);
    }
}
