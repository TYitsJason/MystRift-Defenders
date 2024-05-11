using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{
    public string cardName;
    public int cost;
    public string description;
    public Sprite sprite;
    public int targetCol;
    public int targetRow;
}

public enum AttackType
{
    none,
    normal,
    melee,
    diagonal,
    explosive
}

[CreateAssetMenu(fileName = "New Tower Card", menuName = "New Tower Card")]
public class TowerCard : Card
{
    public int attackPower;
    public int health;
    public AttackType attackType;
    public bool isPlaced = false;
    public PowerCard powerCardVariant;
    public GameObject towerPrefab;
}

public interface ICardActions
{
    void ExecuteAction();
}

[CreateAssetMenu(fileName = "New Power Card", menuName = "New Power Card")]
public class PowerCard : Card
{
    public List<ICardActions> actions;
}

public class DrawCardsAction : ICardActions
{
    public int cardsToDraw;
    public DrawCardsAction(int cardsToDraw)
    {
        this.cardsToDraw = cardsToDraw;
    }
    public void ExecuteAction()
    {
        DeckManager.Instance.DrawCard(cardsToDraw);
    }
}