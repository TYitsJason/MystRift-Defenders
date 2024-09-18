using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Power Up Action", menuName = "Card Effects/Power Up")]
public class PowerUp : CardEffect
{
    public override void Execute(GridCell cell)
    {
        cell.towerObject.GetComponent<TowerObject>().bonusPower++;
    }
}
