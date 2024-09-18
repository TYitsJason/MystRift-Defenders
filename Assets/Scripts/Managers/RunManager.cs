using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RunManager : Singleton<RunManager>
{
    public List<Card> currentDeck = new List<Card>();
    public List<Card> starterDeck = new List<Card>();
    public List<Card> commonCards;
    public List<Card> uncommonCards;
    public List<Card> rareCards;

    public int gold;
    public Text goldText;

    public int currentRegion;
    public int commonChance;
    public int uncommonChance;

    public List<EncounterManager> encounterManagers;

    public Canvas CardOfferUI;
    public Canvas MainMenu;
    public List<CardDisplay> cardOfferObjects;

    public bool canChooseReward;

    public bool hasLifeline;
    private bool lifelineUsedThisTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitializeRun()
    {
        currentRegion = 0;
        gold = 0;
        canChooseReward = false;
        CardOfferUI.gameObject.SetActive(false);
        AdjustRarityThresholds();
        AddInitialCardsToDeck();
        StartNewEncounter();
        hasLifeline = true;
        MainMenu.gameObject.SetActive(false);
        UpdateGold();
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
        if (encounterManagers[currentRegion].completedEncounters.Count == 4 || encounterManagers[currentRegion].completedEncounters.Count == 8)
            StartShopEncounter();
        else if (encounterManagers[currentRegion].completedEncounters.Count == 9)
            StartNextRegion();
        else if (encounterManagers[currentRegion].completedEncounters.Count == 5)
            PlayBetaMessage();
        else
            StartCombatEncounter();
    }

    private void AdjustRarityThresholds()
    {
        // Adjust thresholds based on regions completed
        commonChance = 80 - currentRegion * 20; 
        uncommonChance = 20 + currentRegion * 10; 
    }

    public void OfferNewCards(bool boss)
    {
        CardOfferUI.gameObject.SetActive(true);
        if (!boss)
        {
            AdjustRarityThresholds();
            foreach (CardDisplay cardDisplay in cardOfferObjects)
            {
                Card randomCard = GetRandomCard();
                cardDisplay.card = new CardInstance(randomCard);
                cardDisplay.DisplayCardInfo();
            }
        }
        else
        {
            foreach (CardDisplay cardDisplay in cardOfferObjects)
            {
                Card randomCard = rareCards[Random.Range(0, rareCards.Count)];
                cardDisplay.card = new CardInstance(randomCard);
                cardDisplay.DisplayCardInfo();
            }
        }
        canChooseReward = true;
    }

    public void UpdateGold()
    {
        goldText.text = "Gold: " + gold;
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

    public void StartShopEncounter()
    {
        DeckManager.Instance.InitializeDeck(new List<Card>(currentDeck));
        GridManager.Instance.CreateGrid();
        encounterManagers[currentRegion].StartNewSpawnerEncounter();
        GameManager.Instance.StartGame();
    }

    public void StartNextRegion()
    {
        Debug.Log("To be continued");
    }

    public void StartCombatEncounter()
    {
        DeckManager.Instance.InitializeDeck(new List<Card>(currentDeck));
        GridManager.Instance.CreateGrid();
        encounterManagers[currentRegion].StartNewEncounter();
        GameManager.Instance.StartGame();

    }

    public void LeftButton()
    {
        OnCardSelected(cardOfferObjects[0].card.cardData);
    }
    public void MiddleButton()
    {
        OnCardSelected(cardOfferObjects[1].card.cardData);
    }
    public void RightButton()
    {
        OnCardSelected(cardOfferObjects[2].card.cardData);
    }

    public void SkipSelection()
    {
        canChooseReward = false;
        CardOfferUI.gameObject.SetActive(false);
    }

    public void OnCardSelected(Card selectedCard)
    {
        currentDeck.Add(selectedCard);
        canChooseReward = false;
        CardOfferUI.gameObject.SetActive(false);
    }

    public void UseLifeLine()
    {
        if (!hasLifeline)
        {
            if (!lifelineUsedThisTurn)
            {
                GameManager.Instance.GameOver(false);
                return;
            }
            return;
        }

        lifelineUsedThisTurn = true;
        foreach (GameObject enemyObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemy.TakeDamage(9999);
        }
        Debug.Log("Lifeline used, all enemies killed");
        hasLifeline = false;
    }
    public void PlayBetaMessage()
    {
        GameManager.Instance.GameOver(true);
    }

    public void ResetLifelineUsage()
    {
        lifelineUsedThisTurn = false;
    }
}