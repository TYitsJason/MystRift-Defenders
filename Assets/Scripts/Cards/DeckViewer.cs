using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckViewer : MonoBehaviour
{
    public Button openDeckViewerButton;
    public Button openDiscardViewerButton;
    public Button closeViewerButton;
    public GameObject deckViewerPanel;
    public GameObject cardDisplayPrefab;
    public ScrollRect scrollRect;
    public GridLayoutGroup gridLayoutGroup;
    public CardDisplay detailedCardDisplay;
    public CardDisplay powerCardDisplay;

    public bool deck;

    [SerializeField] private int initialPoolSize = 30;
    private List<CardDisplay> cardDisplayPool = new List<CardDisplay>();
    private List<CardDisplay> activeCardDisplays = new List<CardDisplay>();

    public void OnEnable()
    {
        openDeckViewerButton.onClick.AddListener(ToggleDeckViewer);
        openDiscardViewerButton.onClick.AddListener(ToggleDiscardViewer);
        closeViewerButton.onClick.AddListener(CloseDeckViewer);
        deckViewerPanel.SetActive(false);
        InitializeCardPool();
    }

    private void InitializeCardPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateCardDisplay();
        }
    }

    private CardDisplay CreateCardDisplay()
    {
        GameObject cardObj = Instantiate(cardDisplayPrefab, gridLayoutGroup.transform);
        CardDisplay cardDisplay = cardObj.GetComponent<CardDisplay>();
        Button cardButton = cardObj.GetComponent<Button>();
        cardButton.onClick.AddListener(() => ShowDetailedCard(cardDisplay.card));
        cardObj.SetActive(false);
        cardDisplayPool.Add(cardDisplay);
        return cardDisplay;
    }

    private void ToggleDeckViewer()
    {
        if (deckViewerPanel.activeSelf)
        {
            CloseDeckViewer();
        }
        else
        {
            deck = true;
            OpenDeckViewer();
        }
    }

    private void ToggleDiscardViewer()
    {
        if (deckViewerPanel.activeSelf)
        {
            CloseDeckViewer();
        }
        else
        {
            deck = false;
            OpenDeckViewer();
        }
    }

    private void OpenDeckViewer()
    {
        deckViewerPanel.SetActive(true);
        PopulateDeckList(deck);
        RefreshScrollView();
    }

    private void CloseDeckViewer()
    {
        deckViewerPanel.SetActive(false);
        ClearDeckList();
    }

    private void PopulateDeckList(bool deck)
    {
        ClearDeckList();
        List<CardInstance> cardsToDisplay;
        if (deck)
            cardsToDisplay = DeckManager.Instance.deck;
        else
            cardsToDisplay = DeckManager.Instance.discard;

        while (cardDisplayPool.Count < cardsToDisplay.Count)
        {
            CreateCardDisplay();
        }

        for (int i = 0; i < cardsToDisplay.Count; i++)
        {
            CardDisplay cardDisplay = cardDisplayPool[i];
            cardDisplay.gameObject.SetActive(true);
            cardDisplay.card = cardsToDisplay[i];
            cardDisplay.DisplayCardInfo();
            activeCardDisplays.Add(cardDisplay);
        }
    }

    private void ClearDeckList()
    {
        foreach (CardDisplay display in activeCardDisplays)
        {
            display.gameObject.SetActive(false);
        }
        activeCardDisplays.Clear();
    }

    private void ShowDetailedCard(CardInstance cardInstance)
    {
        detailedCardDisplay.card = cardInstance;
        detailedCardDisplay.DisplayCardInfo();
        detailedCardDisplay.gameObject.SetActive(true);

        if (cardInstance.cardData is TowerCard towerCard)
        {
            powerCardDisplay.card = new CardInstance(towerCard.powerCardVariant);
            powerCardDisplay.DisplayCardInfo();
            powerCardDisplay.gameObject.SetActive(true);
        }
        else
        {
            powerCardDisplay.gameObject.SetActive(false);
        }
    }

    private void RefreshScrollView()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)scrollRect.transform);
        Canvas.ForceUpdateCanvases();

        scrollRect.verticalNormalizedPosition = 1f;

        ContentSizeFitter contentSizeFitter = gridLayoutGroup.GetComponent<ContentSizeFitter>();
        if (contentSizeFitter != null)
        {
            contentSizeFitter.SetLayoutVertical();
        }
    }
}
