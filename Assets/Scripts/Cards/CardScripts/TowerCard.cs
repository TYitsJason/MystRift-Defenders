using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Card", menuName = "New Tower Card")]
public class TowerCard : Card
{
    public int attackPower;
    public int health;
    public AttackType attackType;
    public PowerCard powerCardVariant;
    public GameObject towerPrefab;
}

