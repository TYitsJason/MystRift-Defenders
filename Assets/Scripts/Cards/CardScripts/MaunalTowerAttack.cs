using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Manual Tower Attack", menuName = "Card Effects/Manual Tower Attack")]
public class ManualTowerAttack : CardEffect
{
    public int numOfAttacks;
    public override void Execute(GridCell cell)
    {
        for (int i = 0; i < numOfAttacks; i++)
        {
            cell.towerObject.GetComponent<TowerObject>().Attack();
        }
    }
}