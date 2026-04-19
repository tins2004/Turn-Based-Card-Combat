using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteractionSystem : SingletonMonoBehaviour<CardInteractionSystem>
{
    [Header("Hover Settings")]
    [SerializeField] private LayerMask floorLayer;

    [TagSelector] [SerializeField] private string cardTag;
    
    private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

    private GameObject lastHoveredCell;
    private int cellIndexCanImpact;

    private CardPresenter activeCard;
    private CellSelectionLogic _selectionLogic;
    private GridFloorRepository _gridFloorRepository;
    private ActorOnFloorRepository _actorOnFloorRepository;

    private HashSet<int> enemyCellIndices = new HashSet<int>();
        
    private void Start()
    {
        activeCard = null;
        _selectionLogic = new CellSelectionLogic();

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

    public void UpdateEnemyRange(int[] indices)
    {
        enemyCellIndices.Clear();
        foreach(int i in indices) enemyCellIndices.Add(i);
    }

    private bool UpdateRaycastResults(Vector2 mousePos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = mousePos
        };

        _raycastResults.Clear();
        EventSystem.current.RaycastAll(eventData, _raycastResults);

        if (_raycastResults != null && _raycastResults.Count > 0)
        {
            return true;
        } 

        return false;
    }

    private void HandlePointerDown(Vector2 mousePos, Vector2 worldPos)
    {
        UpdateEnemyRange(_actorOnFloorRepository.GetCellOfActorType(2));

        if (!UpdateRaycastResults(mousePos)) return;

        for (int i = 0; i < _raycastResults.Count; i++)
        {
            var hit = _raycastResults[i];
            
            if (!hit.gameObject.CompareTag(cardTag)) continue; 
            
            activeCard = hit.gameObject.GetComponentInParent<CardPresenter>();
            if (activeCard == null) continue;

            activeCard.SelectedCard(true);

            _selectionLogic.UpdateRealImpact(activeCard.GetRealCellsImpact(_actorOnFloorRepository.GetCellOfActorType(1)[0], 1));
            CellVisualHighlighter.HighlightCells(_selectionLogic.realImpactObjects, Color.blue);
            
            break;
        }
    }

    private void HandlePointerDrag(Vector2 worldPos)
    {
        if (activeCard == null) return;
        
        RaycastHit2D hit = InputSystem.GetHitUnderPosition(worldPos, floorLayer);

        if (hit.collider != null) 
        {   
            GameObject currentCell = hit.collider.gameObject;

            if (currentCell == lastHoveredCell) return;

            ResetAllCellsVisuals();
            lastHoveredCell = currentCell;

            UpdateSelectionVisuals(currentCell, hit.collider.name);
        }
        else
        {
            ResetAllCellsVisuals();
            CellVisualHighlighter.HighlightCells(_selectionLogic.realImpactObjects, Color.blue);
        }
    }

    private void HandlePointerUp(Vector2 worldPos)
    {
        if (activeCard == null) return;
        
        if (lastHoveredCell != null)
        {            
            RaycastHit2D hit = InputSystem.GetHitUnderPosition(worldPos, floorLayer);

            if (hit.collider != null) 
            {
                int cellIndex = GetCellIndexFromCellName(hit.collider.name);

                if (_selectionLogic.realImpactIndices.Contains(cellIndex))
                {
                    Observer.Notify(ObserverEvents.ACTOR_USED_SKILL, new CardActionORD(activeCard._skillStrategy, cellIndexCanImpact));   
                    Observer.Notify(ObserverEvents.CARD_USED, activeCard.gameObject.name); 
                }
            }
        }

        activeCard.SelectedCard(false);
        ResetAllCellsVisuals();

        _selectionLogic.ClearRealImpact();

        activeCard = null;
    }

    private void UpdateSelectionVisuals(GameObject cell, string cellName)
    {
        int cellIndex = GetCellIndexFromCellName(cellName);

        if (_selectionLogic.realImpactIndices.Contains(cellIndex))
        {
            CellVisualHighlighter.HighlightCells(_selectionLogic.realImpactObjects, Color.white);

            var impactIndices = activeCard.GetCellsCanImpact(cellIndex);
            foreach (var idx in impactIndices)
            {
                var obj = _gridFloorRepository.Get(idx);
                cellIndexCanImpact = idx;

                CellVisualHighlighter.HighlightSingle(obj, Color.green);
                _selectionLogic.currentActionAreaObjects.Add(obj);
            }

            if (activeCard.GetCardData().Detail.RangeSkillImpact > 0)
            {
                var lineIndices = activeCard.GetCellsOnLineImpact(cellIndex);
                foreach (var idx in lineIndices)
                {
                    var obj = _gridFloorRepository.Get(idx);
                    CellVisualHighlighter.HighlightSingle(obj, Color.yellow);

                    _selectionLogic.currentActionAreaObjects.Add(obj);
                }
            }
        }
        else
        {
            CellVisualHighlighter.HighlightSingle(cell, Color.red);
        }
    }

    private void ResetAllCellsVisuals()
    {
        CellVisualHighlighter.HighlightCells(_gridFloorRepository.GetAllGameObjectCells(), Color.white);
        _selectionLogic.ClearCurrentActionArea();
        lastHoveredCell = null;
    }

    private int GetCellIndexFromCellName(string cellName)
    {
        if (string.IsNullOrEmpty(cellName)) return 0;
        
        string[] partsName = cellName.Split(' ');
        string lastPart = partsName[partsName.Length - 1];

        if (int.TryParse(lastPart, out int cellIndex)) 
        {
            return cellIndex;
        }

        Debug.LogError("Cell Name is Wrong!");
        return 0;
    }
}
