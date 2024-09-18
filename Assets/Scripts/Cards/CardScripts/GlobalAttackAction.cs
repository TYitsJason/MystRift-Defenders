using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Global Attack Action", menuName = "Card Effects/Global Attack")]
public class GlobalAttack : CardEffect
{
    public override void Execute(GridCell cell)
    {
        GameManager.Instance.GlobalAttack();
    }
}
