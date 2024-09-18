using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Discard Cards Action", menuName = "Card Effects/Discard Cards")]
public class DiscardCardsAction : CardEffect
{
    public int cardsToDiscard;
    public override void Execute(GridCell cell)
    {
        DeckManager.Instance.DiscardCard(cardsToDiscard);
    }
}
