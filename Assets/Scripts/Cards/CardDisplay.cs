using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public CardInstance card;

    public Text nameText;
    public Text descriptionText;
    public Text costText;
    //public Image cardSprite;
    public Text attackText;
    public Text healthText;

    private void Start()
    {
        DisplayCardInfo();
    }

    public void DisplayCardInfo()
    {
        if (card == null)
        {
            return;
        }
        gameObject.SetActive(true);
        if (card.cardData != null)
        {
            nameText.text = card.cardData.cardName;
            descriptionText.text = card.cardData.description;
            //cardSprite.sprite = card.sprite;

            if (card.cardData is TowerCard towerCard && !card.isPlaced)
            {
                attackText.gameObject.SetActive(true);
                healthText.gameObject.SetActive(true);
                attackText.text = towerCard.attackPower.ToString();
                healthText.text = towerCard.health.ToString();
            }
            else
            {
                attackText.gameObject.SetActive(false);
                healthText.gameObject.SetActive(false);
            }
        }
    }

    public void StopDisplay()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (RunManager.Instance.canChooseReward && card != null)
        {
            RunManager.Instance.OnCardSelected(card.cardData);
        }
    }
}
