using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gain Mana Action", menuName = "Card Effects/Gain Mana")]
public class GainManaAction : CardEffect
{
    public int manaToGain;
    public override void Execute(GridCell cell)
    {
        DeckManager.Instance.GainMana(manaToGain);
    }
}
