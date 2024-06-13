using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

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
        deck.Clear();
        foreach (Card card in newDeck)
        {
            deck.Add(new CardInstance(card));
        }
        hand.Clear();
        discard.Clear();
    }

    private void OnPlayerStart()
    {
        ShuffleDeck();
        DrawCard(handSize);
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
        while (hand.Count < maxSize && numToDraw > 0)
        {
            if (deck.Count == 0)
            {
                foreach (var card in discard)
                {
                    deck.Add(card);
                }
                discard.Clear();
                ShuffleDeck();
            }
            hand.Add(deck[0]);
            deck.RemoveAt(0);
        }
        OnHandChanged?.Invoke(hand);
    }

    public void EndTurn()
    {
        // Discard all cards in hand
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
        if (card.cardData.cost > mana)
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
            HandleCardPlay(card);
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
        if (cardInstance.cardData is TowerCard towerCard)
            powerCard = cardInstance.PowerCardVariant;
        else 
            powerCard = cardInstance.cardData as PowerCard;
        if (powerCard.targetTower)
        {
            if (cell.IsOccupiedByTower)
            {
                powerCard.Play(cell);
                HandleCardPlay(cardInstance);
                return true;
            }
            Debug.Log("No tower found while trying to use tower targeting power");
            return false;
        }
        else
        {
            powerCard.Play(cell);
            HandleCardPlay(cardInstance);
            return true;
        }
    }

    public void HandleCardPlay(CardInstance card)
    {
        mana -= card.cardData.cost;
        hand.Remove(card);
        discard.Add(card);
        OnHandChanged?.Invoke(hand);
    }
}
