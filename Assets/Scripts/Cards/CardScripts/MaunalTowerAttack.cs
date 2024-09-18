using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Manual Tower Attack", menuName = "Card Effects/Manual Tower Attack")]
public class ManualTowerAttack : CardEffect
{
    public override void Execute(GridCell cell)
    {
        cell.towerObject.GetComponent<TowerObject>().manual = true;
        cell.towerObject.GetComponent<TowerObject>().Attack();
        cell.towerObject.GetComponent<TowerObject>().manual = false;
    }
}