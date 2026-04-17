using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private List<int> cellsCanImpactInt = new List<int>();
    private List<int> realCellsImpactInt = new List<int>();
    private List<int> cellsOnLineImpactInt = new List<int>();
    private List<GameObject> realImpactCells = new List<GameObject>();
    private List<GameObject> onLineLastHoveredCells = new List<GameObject>();
    #endregion

    private CardPresenter cardPresenter;
    private GridFloorRepository _gridFloorRepository;
    private ActorOnFloorRepository _actorOnFloorRepository;

    private void Start()
    {
        cardPresenter = null;
        _gridFloorRepository = GridFloorRepository.Instance;
        _actorOnFloorRepository = ActorOnFloorRepository.Instance;
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

                realCellsImpactInt = cardPresenter.GetRealCellsImpact(_actorOnFloorRepository.GetCellOfActorType(1)[0], 1);
                foreach (int cellImpact in realCellsImpactInt)
                {
                    GameObject obj = _gridFloorRepository.Get(cellImpact);
                    if (obj == null) continue;
                    realImpactCells.Add(obj);
                }

                NotifyRealCellsImpact(ObserverEvents.CELL_HOVERED, Color.blue);
                

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
                                        inLineColor: Color.yellowGreen);
            }
        }
        else
        {
            NotifyCellMouseExit(ObserverEvents.CELL_HOVERED, Color.white);
            NotifyRealCellsImpactExitColor(ObserverEvents.CELL_HOVERED, Color.blue);
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

                if (realCellsImpactInt.Contains(cellIndex))
                {
                    Observer.Notify(ObserverEvents.USED_CARD, new CardActionORD(cardPresenter._skillStrategy, lastHoveredCellIndex));   
                    Observer.Notify(ObserverEvents.CHOOSE_CARD, cardPresenter.gameObject.name); 
                }
            }

            NotifyCellMouseExit(ObserverEvents.CELL_HOVERED, Color.white);
        }
        
        cardPresenter = null;
        NotifyRealCellsImpactClear(ObserverEvents.CELL_HOVERED, Color.white);
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
        NotifyRealCellsImpactExitColor(observerEvents, Color.white);

        int cellIndex = GetCellIndexFromCellName(cellName);

        if (realCellsImpactInt.Contains(cellIndex))
        {
            cellsCanImpactInt = cardPresenter.GetCellsCanImpact(cellIndex);

            if (cellsCanImpactInt != null && cellsCanImpactInt.Count > 0)
            {
                foreach (int cellCanImpact in cellsCanImpactInt)
                {  
                    GameObject obj = _gridFloorRepository.Get(cellCanImpact);
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
                cellsOnLineImpactInt = cardPresenter.GetCellsOnLineImpact(cellIndex);

                foreach (int cellOnLineImpact in cellsOnLineImpactInt)
                {  
                    GameObject obj = _gridFloorRepository.Get(cellOnLineImpact);
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

        if (cellsCanImpactInt != null && cellsCanImpactInt.Count > 0)
            cellsCanImpactInt.Clear();

        if (cellsOnLineImpactInt != null && cellsOnLineImpactInt.Count > 0)
            cellsOnLineImpactInt.Clear();
    }

    private void NotifyRealCellsImpact(string observerEvents, Color color)
    {   
        if (realImpactCells != null && realImpactCells.Count > 0)
        {
            foreach (GameObject cellOnLineImpact in realImpactCells)
            {
                Observer.Notify(observerEvents, new CellHoveredORD(cellOnLineImpact, color));
            }
        }
    }

    private void NotifyRealCellsImpactExitColor(string observerEvents, Color exitColor)
    {   
        if (realImpactCells != null && realImpactCells.Count > 0)
        {
            foreach (GameObject cellOnLineImpact in realImpactCells)
            {
                Observer.Notify(observerEvents, new CellHoveredORD(cellOnLineImpact, exitColor));
            }
        }
    }

    private void NotifyRealCellsImpactClear(string observerEvents, Color exitColor)
    {
        if (realImpactCells != null && realImpactCells.Count > 0)
        {
            NotifyRealCellsImpactExitColor(observerEvents, exitColor);

            realImpactCells.Clear();
        }

        if (realCellsImpactInt != null && realCellsImpactInt.Count > 0)
            realCellsImpactInt.Clear();
    }
}
