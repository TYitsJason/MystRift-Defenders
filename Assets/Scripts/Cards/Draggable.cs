using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 originalAnchoredPosition;
    private Camera mainCamera;
    public LayerMask placementLayer;
    private bool isDragging = false;

#if UNITY_ANDROID || UNITY_IOS
    private Vector2 touchStartPosition;
#endif

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        if (eventData.button != PointerEventData.InputButton.Left) return;
#endif

        isDragging = true;
        originalAnchoredPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

#if UNITY_ANDROID || UNITY_IOS
        touchStartPosition = eventData.position;
#endif
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
#elif UNITY_ANDROID || UNITY_IOS
        Vector2 touchDelta = eventData.position - touchStartPosition;
        rectTransform.anchoredPosition = originalAnchoredPosition + touchDelta / canvas.scaleFactor;
#endif
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        Vector2 endPosition = eventData.position;
        bool shouldPlayCard = false;

#if UNITY_ANDROID || UNITY_IOS
        float dragDistance = Vector2.Distance(touchStartPosition, endPosition);
        shouldPlayCard = dragDistance > 10f; 
#else
        shouldPlayCard = true;
#endif

        if (shouldPlayCard)
        {
            Ray ray = mainCamera.ScreenPointToRay(endPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
            {
                GridCell cell = hit.collider.GetComponentInParent<GridCell>();
                Debug.Log("Playing card at cell: " + cell.X + "," + cell.Y);
                DeckManager.Instance.PlayCard(GetComponent<CardDisplay>().card, cell);
            }
        }

        rectTransform.anchoredPosition = originalAnchoredPosition;
    }
}