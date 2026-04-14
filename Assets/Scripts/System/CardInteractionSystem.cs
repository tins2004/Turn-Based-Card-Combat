using System.Collections.Generic;
using UnityEngine;

public class CardInteractionSystem : SingletonMonoBehaviour<CardInteractionSystem>
{
    [Header("Hover Settings")]
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private LayerMask cardLayer;

    private GameObject lastHoveredCell;
    private List<GameObject> onLineLastHoveredCells = new List<GameObject>();
    private List<int> cellsCanImpact = new List<int>();

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

    private void HandlePointerDown(Vector2 worldPos)
    {
        RaycastHit2D hit = InputSystem.GetHitUnderPosition(worldPos, cardLayer);

        if (hit.collider != null) 
        {
            cardPresenter = hit.collider.GetComponent<CardPresenter>();
            if (cardPresenter != null)
            {
                cardPresenter.SelectedCard(true);
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

            if (hit.collider != null) 
            {
                foreach (int cellCanImpact in cellsCanImpact)
                {
                    int cellIndex = GetCellIndexFromCellName(hit.collider.name);
                    
                    if (cellCanImpact == cellIndex)
                    {
                        Observer.Notify(ObserverEvents.SELECTED_CELL, cellIndex);   
                    }
                }
            }

            NotifyCellMouseExit(ObserverEvents.CELL_HOVERED, Color.white);
        }
        
        cardPresenter = null;
        lastHoveredCell = null;
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
        
        cellsCanImpact = cardPresenter.GetCellsCanImpact();
        foreach (int cellCanImpact in cellsCanImpact)
        {
            if (cellCanImpact == GetCellIndexFromCellName(cellName))
            {
                Observer.Notify(observerEvents, new CellHoveredORD(cell, rightColor));

                if (cardPresenter.rangeCardImpact > 1)
                {
                    List<int> cellsOnLineImpact = cardPresenter.GetCellsOnLineImpact(GetCellIndexFromCellName(cellName));

                    foreach (int cellOnLineImpact in cellsOnLineImpact)
                    {  
                        GameObject obj = GridFloorRepository.Instance.Get(cellOnLineImpact);

                        Observer.Notify(observerEvents, new CellHoveredORD(obj, inLineColor));
                        onLineLastHoveredCells.Add(obj);
                    }
                }

                return;
            }
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

        if (onLineLastHoveredCells != null && onLineLastHoveredCells.Count > 0)
        {
            foreach (GameObject cellOnLineImpact in onLineLastHoveredCells)
            {
                Observer.Notify(observerEvents, new CellHoveredORD(cellOnLineImpact, exitColor));
            }

            onLineLastHoveredCells.Clear();
        }
    }
}
