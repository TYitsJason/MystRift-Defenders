using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Draw Cards Action", menuName = "Card Effects/Draw Cards")]
public class DrawCardsAction : CardEffect
{
    public int cardsToDraw;
    public override void Execute(GridCell cell)
    {
        DeckManager.Instance.DrawCard(cardsToDraw);
    }
}
