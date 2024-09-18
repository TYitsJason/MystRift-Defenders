using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTower : TowerObject
{
    public override void Attack()
    {
        TowerCard towerCard = card.cardData as TowerCard;
        if (Health < towerCard.health)
            Health += Power + bonusPower;
    }
}
