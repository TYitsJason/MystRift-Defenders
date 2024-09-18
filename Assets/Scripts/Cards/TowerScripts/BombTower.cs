using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTower : TowerObject
{
    public GameObject explosionRadius;
    public Explosion explosionReference;
    public override void Attack()
    {
        explosionReference = Instantiate(explosionRadius, transform).GetComponent<Explosion>();
        explosionReference.damage = Power + bonusPower;
    }
}
