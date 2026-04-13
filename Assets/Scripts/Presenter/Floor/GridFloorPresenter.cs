using UnityEngine;
using UnityEngine.InputSystem;

public class GridFloorPresenter : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width = 3;
    [SerializeField] private float cellWidth = 3;
    [SerializeField] private float cellHeight = 3;

    [Header("Hover Settings")]
    [SerializeField] private LayerMask cellFloorLayer;

    private GridFloorView _view;
    private GridFloorModel _model;

    private GameObject lastHoveredCell;
    private bool hadSelectedCard;

    private void Awake()
    {
        if (_view == null)
        {
            _view = GetComponent<GridFloorView>();
        }
    }

    private void Start()
    {
        _model = new GridFloorModel(width, cellWidth, cellHeight);
        GenerateGrid();

        SetupObserverListener();
    }

    private void GenerateGrid()
    {

        for (int x = 0; x < _model.width; x++)
        {
            _view.SpawnCell(_model.GetCellPosition(x), new Vector3(_model.cellWidth, _model.cellHeight, 1f), $"{_model.cellName} {x}");
        
            GridFloorRepository.Instance.Add(x, _model.GetCellPosition(x).x);
        }
    }

    private void HandleInteraction()
    {
        if (!hadSelectedCard)
        {
            if(lastHoveredCell != null)
            {
                _view.UpdateCellVisual(lastHoveredCell, false);
                lastHoveredCell = null;
            }

            return;
        }

        RaycastHit2D hit = _view.GetHitUnderMouse(cellFloorLayer);


        if (hit.collider != null)
        {
            GameObject currentObject = hit.collider.gameObject;

            if (currentObject != lastHoveredCell)
            {
                if(lastHoveredCell != null)
                {
                    _view.UpdateCellVisual(lastHoveredCell, false);
                }

                _view.UpdateCellVisual(currentObject, true);

                lastHoveredCell = currentObject;
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                SelectCell(hit.collider.name);
            }
        }
        else
        {
            if(lastHoveredCell != null)
            {
                _view.UpdateCellVisual(lastHoveredCell, false);
                lastHoveredCell = null;
            }
        }
    }

    private void SelectCell(string cellName)
    {
        string numberStr = cellName[^1].ToString();

        if (int.TryParse(numberStr, out int cellIndex))
        {
            Observer.Notify(ObserverEvents.ON_CLICK_CELL, cellIndex);
        }
    }

    private void HandleCardSelected(object data)
    {
        hadSelectedCard = (bool)data;
    }

    private void OnEnable()
    {
        _view.OnMouseInteraction += HandleInteraction;
    }

    private void OnDisable()
    {
        _view.OnMouseInteraction -= HandleInteraction;
    }

    private void SetupObserverListener()
    {
        Observer.AddListener(ObserverEvents.SELECTED_CARD, HandleCardSelected);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(ObserverEvents.SELECTED_CARD, HandleCardSelected);
    }
}
