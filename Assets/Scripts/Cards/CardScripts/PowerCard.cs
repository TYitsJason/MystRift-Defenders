using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Power Card", menuName = "New Power Card")]
public class PowerCard : Card
{
    public List<CardEffect> actions;
    public bool targetTower = false;
    public void Play(GridCell cell)
    {
        foreach (CardEffect effect in actions)
            effect.Execute(cell);
    }
}