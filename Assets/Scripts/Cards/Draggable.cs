using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    public Vector3 originalPos;

    private Camera mainCamera;
    public LayerMask placementLayer;
    private CardInstance attachedCard;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        mainCamera = FindAnyObjectByType<Camera>();
        attachedCard = GetComponent<CardDisplay>().card;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPos = rectTransform.position;
        canvasGroup.alpha = 0.1f; // Makes the selected card semi-transparent, replace this function later
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            GridCell cell = hit.collider.GetComponentInParent<GridCell>();
            Debug.Log(cell);
            if (DeckManager.Instance.PlayCard(attachedCard, cell))
                gameObject.SetActive(false);
            transform.position = originalPos;
        }
    }
}
