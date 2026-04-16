using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class CardInteractionSystem : SingletonMonoBehaviour<CardInteractionSystem>
{
    [Header("Hover Settings")]
    [SerializeField] private LayerMask floorLayer;

    [TagSelector] [SerializeField] private string cardTag;
    
    private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

    #region(cell impact)
    private GameObject lastImpactCell;
    private int lastHoveredCellIndex;
    private GameObject lastHoveredCell;
    private List<int> cellsCanImpact = new List<int>();
    private List<int> realCellsImpact = new List<int>();
    private List<int> cellsOnLineImpact = new List<int>();
    private List<GameObject> onLineLastHoveredCells = new List<GameObject>();
    #endregion

    private CardPresenter cardPresenter;

    private void Start()
    {
        cardPresenter = null;
    }

    private void OnEnable()
    {
        InputSystem.OnPointerDown += HandlePointerDown;
        InputSystem.OnPointerDrag += HandlePointerDrag;
        InputSystem.OnPointerUp += HandlePointerUp;
    }

    private void OnDisable()
    {
        InputSystem.OnPointerDown -= HandlePointerDown;
        InputSystem.OnPointerDrag -= HandlePointerDrag;
        InputSystem.OnPointerUp -= HandlePointerUp;
    }

    private void HandlePointerDown(Vector2 mousePos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = mousePos
        };

        _raycastResults.Clear();
        EventSystem.current.RaycastAll(eventData, _raycastResults);

        for (int i = 0; i < _raycastResults.Count; i++)
        {
            var hit = _raycastResults[i];
            
            if (!hit.gameObject.CompareTag(cardTag)) continue; 
            
            cardPresenter = hit.gameObject.GetComponentInParent<CardPresenter>();
            
            if (cardPresenter != null)
            {
                cardPresenter.SelectedCard(true);
                break;
            }
        }
    }

    private void HandlePointerDrag(Vector2 worldPos)
    {
        if (cardPresenter == null) return;

        RaycastHit2D hit = InputSystem.GetHitUnderPosition(worldPos, floorLayer);

        if (hit.collider != null) 
        {   
            GameObject currentObject = hit.collider.gameObject;


            if (currentObject != lastHoveredCell)
            {
                NotifyCellMouseExit(ObserverEvents.CELL_HOVERED, Color.white);

                lastHoveredCell = currentObject;

                NotifyCellHovered(currentObject, hit.collider.name, ObserverEvents.CELL_HOVERED, 
                                        rightColor: Color.green,
                                        wrongColor: Color.red,
                                        inLineColor: Color.yellow);
            }
        }
        else
        {
            NotifyCellMouseExit(ObserverEvents.CELL_HOVERED, Color.white);
        }
    }

    private void HandlePointerUp(Vector2 worldPos)
    {
        if (cardPresenter == null) return;
        
        cardPresenter.SelectedCard(false);

        if (lastHoveredCell != null)
        {            
            RaycastHit2D hit = InputSystem.GetHitUnderPosition(worldPos, floorLayer);

            if (hit.collider != null && lastImpactCell != null) 
            {
                int cellIndex = GetCellIndexFromCellName(hit.collider.name);

                if (realCellsImpact.Contains(cellIndex))
                {
                    Observer.Notify(ObserverEvents.USED_CARD, new CardActionORD(cardPresenter._skillStrategy, lastHoveredCellIndex));   
                    Observer.Notify(ObserverEvents.CHOOSE_CARD, cardPresenter.gameObject.name); 
                }
            }

            NotifyCellMouseExit(ObserverEvents.CELL_HOVERED, Color.white);
        }
        
        cardPresenter = null;
    }

    private int GetCellIndexFromCellName(string cellName)
    {
        string[] partsName = cellName.Split(' ');
        string lastPart = partsName[partsName.Length - 1];

        if (int.TryParse(lastPart, out int cellIndex)) 
        {
            return cellIndex;
        }

        Debug.LogError("Cell Name is Wrong!");
        return 0;
    }
    
    private void NotifyCellHovered(GameObject cell, string cellName, string observerEvents, Color rightColor, Color wrongColor, Color inLineColor)
    {
        
        realCellsImpact = cardPresenter.GetRealCellsImpact();
        int cellIndex = GetCellIndexFromCellName(cellName);

        if (realCellsImpact.Contains(cellIndex))
        {
            cellsCanImpact = cardPresenter.GetCellsCanImpact(cellIndex);

            if (cellsCanImpact != null && cellsCanImpact.Count > 0)
            {
                foreach (int cellCanImpact in cellsCanImpact)
                {  
                    GameObject obj = GridFloorRepository.Instance.Get(cellCanImpact);
                    if (obj == null) continue;

                    Observer.Notify(observerEvents, new CellHoveredORD(obj, rightColor));
                    lastImpactCell = obj;
                    lastHoveredCellIndex = cellCanImpact;
                }
            }
            // else
            // {
            //     Observer.Notify(observerEvents, new CellHoveredORD(cell, rightColor));
            //     lastImpactCell = cell;
            //     lastHoveredCellIndex = cellIndex;
            // }

            if (cardPresenter.GetCardData().Detail.RangeSkillImpact > 0)
            {
                cellsOnLineImpact = cardPresenter.GetCellsOnLineImpact(cellIndex);

                foreach (int cellOnLineImpact in cellsOnLineImpact)
                {  
                    GameObject obj = GridFloorRepository.Instance.Get(cellOnLineImpact);
                    if (obj == null) continue;

                    Observer.Notify(observerEvents, new CellHoveredORD(obj, inLineColor));
                    onLineLastHoveredCells.Add(obj);
                }
            }

            return;
        }

        Observer.Notify(observerEvents, new CellHoveredORD(cell, wrongColor));
    }

    private void NotifyCellMouseExit(string observerEvents, Color exitColor)
    {
        if(lastHoveredCell != null)
        {
            Observer.Notify(observerEvents, new CellHoveredORD(lastHoveredCell, exitColor));
        
            lastHoveredCell = null;
        }

        if(lastImpactCell != null)
        {
            Observer.Notify(observerEvents, new CellHoveredORD(lastImpactCell, exitColor));
        
            lastImpactCell = null;
        }

        if (onLineLastHoveredCells != null && onLineLastHoveredCells.Count > 0)
        {
            foreach (GameObject cellOnLineImpact in onLineLastHoveredCells)
            {
                Observer.Notify(observerEvents, new CellHoveredORD(cellOnLineImpact, exitColor));
            }

            onLineLastHoveredCells.Clear();
        }

        if (realCellsImpact != null && realCellsImpact.Count > 0)
            realCellsImpact.Clear();

        if (cellsCanImpact != null && cellsCanImpact.Count > 0)
            cellsCanImpact.Clear();

        if (cellsOnLineImpact != null && cellsOnLineImpact.Count > 0)
            cellsOnLineImpact.Clear();
    }
}
