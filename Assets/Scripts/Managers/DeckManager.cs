using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    public List<Card> deck = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> discard = new List<Card>();

    public GameObject cardPrefab;

    public int handSize = 5;
    public int maxSize = 10;

    public delegate void HandChanged(List<Card> hand);
    public static event HandChanged OnHandChanged;

    protected virtual void OnEnable()
    {
        GameManager.OnPlayerAct += OnPlayerStart;
    }

    protected virtual void OnDisable()
    {
        GameManager.OnPlayerAct -= OnPlayerStart;
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
            Card temp = deck[i];
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
    }

    public void PlayCard(Card card)
    {
        // Implement the logic to play a card here
        // Remember to check if the player has enough mana and to place tower cards on the grid

        // After playing, move the card to the discard pile
        hand.Remove(card);
        discard.Add(card);
        OnHandChanged?.Invoke(hand);
        // Optionally, you might want to redraw immediately after playing a card
        // DrawHand(); // Uncomment this line if you want the player to always have 5 cards in hand
    }
}
