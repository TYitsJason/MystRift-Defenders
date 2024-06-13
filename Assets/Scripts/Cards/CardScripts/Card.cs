using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{
    public string cardName;
    public int cost;
    public string description;
    //public Sprite sprite;
}

public enum AttackType
{
    none,
    normal,
    melee,
    diagonal,
    explosive
}

