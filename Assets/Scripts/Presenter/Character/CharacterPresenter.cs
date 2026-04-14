using UnityEngine;

public class CharacterPresenter : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int spawnPos = 0;

    private CharacterView _view;
    private CharacterModel _model;

    private void Awake()
    {
        if (_view == null)
        {
            _view = GetComponent<CharacterView>();
        }
    }

    private void Start()
    {
        _model = new CharacterModel();

        MoveToCell(spawnPos);

        SetupObserverListener();
    }

    private void MoveToCell(int cell)
    {
        _view.ChangePosition(_model.GetTargetPosition(cell));

        if (_model._actorOnFloorRepository.Exists(_model.currentCharacterCell))
        {
            _model._actorOnFloorRepository.Add(_model.currentCharacterCell, 0);
        }

        _model.currentCharacterCell = cell;
        _model._actorOnFloorRepository.Add(cell, 1);
    }

    private void HandleCelectedCell(object data)
    {
        MoveToCell((int)data);
    }

    private void SetupObserverListener()
    {
        Observer.AddListener(ObserverEvents.SELECTED_CELL, HandleCelectedCell);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(ObserverEvents.SELECTED_CELL, HandleCelectedCell);
    }
}
