using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplayManager : MonoBehaviour
{
    public List<CardDisplay> cardDisplays;
    public GameObject cardPrefab;

    private void Start()
    {
        InitializePrefabs();
    }

    void InitializePrefabs()
    {
        cardDisplays = new List<CardDisplay>();
        for (int i = 0; i < DeckManager.Instance.maxSize; i++)
        {
            GameObject temp = Instantiate(cardPrefab, parent: GetComponent<Transform>());
            cardDisplays.Add(temp.GetComponent<CardDisplay>());
            cardDisplays[i].StopDisplay();
        }
    }

    private void OnEnable()
    {
        DeckManager.OnHandChanged += UpdateHandDisplay;
    }
    private void OnDisable()
    {
        DeckManager.OnHandChanged -= UpdateHandDisplay;
    }

    public void UpdateHandDisplay(List<CardInstance> hand)
    {
        for (int i = 0; i < DeckManager.Instance.maxSize; i++)
        {
            cardDisplays[i].StopDisplay();
        }
        for (int i = 0; i < hand.Count; i++)
        {
            cardDisplays[i].card = hand[i];
            cardDisplays[i].DisplayCardInfo();
        }
    }
}
