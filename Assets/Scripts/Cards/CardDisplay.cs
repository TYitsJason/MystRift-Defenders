using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    public Text nameText;
    public Text descriptionText;
    public Text costText;
    public Image cardSprite;
    public Text attackText;
    public Text healthText;

    private void Start()
    {
        DisplayCardInfo();
    }

    public void DisplayCardInfo()
    {
        gameObject.SetActive(true);
        nameText.text = card.cardName;
        descriptionText.text = card.description;
        cardSprite.sprite = card.sprite;

        if (card is TowerCard towerCard && !towerCard.isPlaced)
        {
            attackText.text = towerCard.attackPower.ToString();
            healthText.text = towerCard.health.ToString();
        }
        else
        {
            attackText.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
        }
    }

    public void StopDisplay()
    {
        gameObject.SetActive(false);
    }
}
