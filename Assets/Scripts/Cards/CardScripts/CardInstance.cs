using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardInstance
{
    public Card cardData;
    public bool isPlaced;
    public PowerCard PowerCardVariant;

    public CardInstance(Card cardData)
    {
        this.cardData = cardData;
        this.isPlaced = false;
        if (cardData is TowerCard towerCard)
        {
            this.PowerCardVariant = towerCard.powerCardVariant;
        }
    }
}