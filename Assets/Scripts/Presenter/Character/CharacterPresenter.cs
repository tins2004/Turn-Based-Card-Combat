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
        _model = new CharacterModel(spawnPos);

        _view.Moving(_model.GetTargetPosition(spawnPos));

        SetupObserverListener();
    }

    private void OnClickCell(object data)
    {
        _view.Moving(_model.GetTargetPosition((int)data));
    }

    private void SetupObserverListener()
    {
        Observer.AddListener(ObserverEvents.ON_CLICK_CELL, OnClickCell);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(ObserverEvents.ON_CLICK_CELL, OnClickCell);
    }
}
