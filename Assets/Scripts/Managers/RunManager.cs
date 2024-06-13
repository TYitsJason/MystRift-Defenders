using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : Singleton<RunManager>
{
    public List<Card> currentDeck = new List<Card>();
    public List<Card> starterDeck = new List<Card>();
    public List<Card> commonCards;
    public List<Card> uncommonCards;
    public List<Card> rareCards;

    public int currentRegion;
    public int commonChance;
    public int uncommonChance;

    public Canvas CardOfferUI;
    public List<CardDisplay> cardOfferObjects;

    public bool canChooseReward;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeRun();
    }

    void InitializeRun()
    {
        currentRegion = 0;
        canChooseReward = false;
        CardOfferUI.gameObject.SetActive(false);
        AdjustRarityThresholds();
        AddInitialCardsToDeck();
        DeckManager.Instance.InitializeDeck(new List<Card>(currentDeck));
    }
    void AddInitialCardsToDeck()
    {
        foreach (Card card in starterDeck)
        {
            currentDeck.Add(card);
        }
    }
    public void StartNewEncounter()
    {
        DeckManager.Instance.InitializeDeck(new List<Card>(currentDeck));
    }

    public void EndEncounter()
    {
        OfferNewCards();
    }

    private void AdjustRarityThresholds()
    {
        // Adjust thresholds based on regions completed
        commonChance = 80 - currentRegion * 20; 
        uncommonChance = 20 + currentRegion * 10; 
    }

    private void OfferNewCards()
    {
        AdjustRarityThresholds();

        CardOfferUI.gameObject.SetActive(true);
        foreach (CardDisplay cardDisplay in cardOfferObjects)
        {
            Card randomCard = GetRandomCard();
            cardDisplay.card = new CardInstance(randomCard);
            cardDisplay.DisplayCardInfo();
        }
        canChooseReward = true;
    }

    private Card GetRandomCard()
    {
        int roll = Random.Range(1, 101);

        if (roll <= commonChance)
        {
            return commonCards[Random.Range(0, commonCards.Count)];
        }
        else if (roll <= uncommonChance)
        {
            return uncommonCards[Random.Range(0, uncommonCards.Count)];
        }
        else
        {
            return rareCards[Random.Range(0, rareCards.Count)];
        }
    }

    public void OnCardSelected(Card selectedCard)
    {
        currentDeck.Add(selectedCard);
        canChooseReward = false;
        CardOfferUI.gameObject.SetActive(false);
    }
}