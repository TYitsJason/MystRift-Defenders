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
    private Card attachedCard;

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
            PlaceObject(cell);
        }
        else
        {
            transform.position = GetComponent<Draggable>().originalPos;
        }
    }

    private bool PlaceObject(GridCell cell)
    {
        if (cell != null)
        {
            if (TryToPlaceTower(cell))
            {
                Debug.Log("Tower placed successfully at: " + cell.X + ", " + cell.Y);
                return true;
            }
            else
            {
                Debug.Log("Failed to place tower at: " + cell.X + ", " + cell.Y);
                return false;
            }
        }
        return false;
    }

    private bool TryToPlaceTower(GridCell cell)
    {
        Debug.Log("Attempting to place tower");
        attachedCard.targetCol = cell.X;
        attachedCard.targetRow = cell.Y;
        if (cell.CanPlaceTower && !cell.IsOccupiedByTower && !cell.IsOccupiedByEnemyStructure)
        {
            if (attachedCard is TowerCard TowerCard && !TowerCard.isPlaced)
                return cell.TryPlaceTower(TowerCard);
            return ActivatePower(cell, (PowerCard)attachedCard);
        }
        else if (cell.CanActivatePower)
            return ActivatePower(cell, (PowerCard)attachedCard);
        return false;
    }

    private bool ActivatePower(GridCell cell, PowerCard powerCard)
    {
        Debug.Log("Attempting to activate power");
        if (cell.CanActivatePower)
        {
            foreach (ICardActions action in powerCard.actions)
            {
                action.ExecuteAction();
            }
            Debug.Log("Successful");
            return true;
        }
        Debug.Log("Failed");
        return false;
    }
}
