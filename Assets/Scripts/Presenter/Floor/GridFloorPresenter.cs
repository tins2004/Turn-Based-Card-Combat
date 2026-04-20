using UnityEngine;

public class GridFloorPresenter : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width = 9;
    [SerializeField] private float cellWidth = 1.5f;
    [SerializeField] private float cellHeight = 0.7f;

    private GridFloorView _view;
    private GridFloorModel _model;

    private void Awake()
    {
        if (_view == null)
        {
            _view = GetComponent<GridFloorView>();
        }

        _model = new GridFloorModel(width, cellWidth, cellHeight);
        GenerateGrid();
    }

    private void Start()
    {
        SetupObserverListener();
    }

    private void GenerateGrid()
    {

        for (int x = 0; x < _model.width; x++)
        {
            GameObject cell = _view.SpawnCell(_model.GetCellPosition(x), new Vector3(_model.cellWidth, _model.cellHeight, 1f), $"{_model.cellName} {x}");
            _view.UpdateCellVisual(cell, Color.white);  
            GridFloorRepository.Instance.Add(x, cell);
            ActorOnFloorRepository.Instance.Add(x, new ActorOnFloorData { actorType = 0, actorObject = null });
        }
    }

    private void HandleCellHovered(object data)
    {
        if (data is CellHoveredORD hoveredData)
        {
            _view.UpdateCellVisual(hoveredData.CellObject, hoveredData.ColorCell);
        }
    }

    private void SetupObserverListener()
    {
        Observer.AddListener(ObserverEvents.CELL_HOVERED, HandleCellHovered);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(ObserverEvents.CELL_HOVERED, HandleCellHovered);
    }
}
