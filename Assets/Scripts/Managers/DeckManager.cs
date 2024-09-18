using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : Singleton<DeckManager>
{
    public List<CardInstance> deck = new List<CardInstance>();
    public List<CardInstance> hand = new List<CardInstance>();
    public List<CardInstance> discard = new List<CardInstance>();

    public GameObject cardPrefab;

    public int handSize = 5;
    public int maxSize = 10;

    public delegate void HandChanged(List<CardInstance> hand);
    public static event HandChanged OnHandChanged;

    public int mana = 3;
    public int maxMana = 3;

    public Text manaDisplay;
    public Canvas GUI;

    protected virtual void OnEnable()
    {
        GameManager.OnPlayerAct += OnPlayerStart;
    }

    protected virtual void OnDisable()
    {
        GameManager.OnPlayerAct -= OnPlayerStart;
    }

    public void InitializeDeck(List<Card> newDeck)
    {
        Debug.Log("Deck manager initalizing deck");
        deck.Clear();
        foreach (Card card in newDeck)
        {
            deck.Add(new CardInstance(card));
        }
        hand.Clear();
        discard.Clear();
        mana = maxMana;
        GUI.gameObject.SetActive(true);
    }

    private void OnPlayerStart()
    {
        ShuffleDeck();
        DrawCard(handSize);
        manaDisplay.text = "Mana " + mana + "/" + maxMana;
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            CardInstance temp = deck[i];
            int randomIndex = UnityEngine.Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public void DrawCard(int numToDraw)
    {
        Debug.Log("Drawing " +  numToDraw + " cards");
        while (hand.Count < maxSize && numToDraw > 0)
        {
            if (deck.Count == 0)
            {
                if (discard.Count == 0)
                {
                    OnHandChanged?.Invoke(hand);
                    return;
                }
                foreach (var card in discard)
                {
                    deck.Add(card);
                }
                discard.Clear();
                ShuffleDeck();
            }
            hand.Add(deck[0]);
            deck.RemoveAt(0);
            numToDraw--;
        }
        OnHandChanged?.Invoke(hand);
    }

    public void DiscardCard(int numToDiscard)
    {
        Debug.Log("Discarding " + numToDiscard + " cards");
        while (hand.Count > 0 && numToDiscard > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, deck.Count);
            discard.Add(hand[randomIndex]);
            hand.RemoveAt(randomIndex);
            numToDiscard--;
        }
        OnHandChanged?.Invoke(hand);
    }

    public void EndTurn()
    {
        // Discard all cards in hand
        Debug.Log("Ending Turn");
        foreach (var card in hand)
        {
            discard.Add(card);
        }
        hand.Clear();
        OnHandChanged?.Invoke(hand);
        mana = maxMana;
    }

    public bool PlayCard(CardInstance card, GridCell cell)
    {
        if (!hand.Contains(card))
        {
            Debug.Log("Card does not exist");
            return false;
        }
        if (card.cardData.cost > mana || (card.cardData is TowerCard tower && card.isPlaced && tower.powerCardVariant.cost > mana))
        {
            Debug.Log("Not enough mana to play");
            return false;
        }
        if (card.cardData is TowerCard towerCard)
        {
            if (card.isPlaced)
            {
                Debug.Log("Playing power card variant");
                return PlayPowerCard(card, cell);
            }
            if (!cell.CanPlaceTower)
            {
                Debug.Log("Cell is not valid target for tower");
                return false;
            }
            if (cell.IsOccupiedByTower)
            {
                Debug.Log("Cell already has a tower");
                return false;
            }
            if (cell.IsOccupiedByEnemyStructure)
            {
                Debug.Log("Cell contains enemy structure");
                return false;
            }
            cell.TryPlaceTower(card);
            HandleCardPlay(card, card.cardData.cost);
            return true;
        }
        else if (card.cardData is PowerCard powerCard)
        {
            return PlayPowerCard(card, cell);
        }
        Debug.Log("This should never run");
        return false;
    }

    public bool PlayPowerCard(CardInstance cardInstance, GridCell cell)
    {
        PowerCard powerCard;
        int manaCost;
        if (cardInstance.cardData is TowerCard towerCard)
            powerCard = cardInstance.PowerCardVariant;
        else
            powerCard = cardInstance.cardData as PowerCard;
        manaCost = powerCard.cost;
        if (powerCard.targetTower)
        {
            if (cell.IsOccupiedByTower)
            {
                powerCard.Play(cell);
                HandleCardPlay(cardInstance, manaCost);
                return true;
            }
            Debug.Log("No tower found while trying to use tower targeting power");
            return false;
        }
        else
        {
            powerCard.Play(cell);
            HandleCardPlay(cardInstance, manaCost);
            return true;
        }
    }

    public void HandleCardPlay(CardInstance card, int manaCost)
    {
        mana -= card.cardData.cost;
        manaDisplay.text = "Mana " + mana + "/" + maxMana;
        hand.Remove(card);
        discard.Add(card);
        OnHandChanged?.Invoke(hand);
    }

    public void GainMana(int manaToGain)
    {
        mana += manaToGain;
        manaDisplay.text = "Mana " + mana + "/" + maxMana;
    }
}
